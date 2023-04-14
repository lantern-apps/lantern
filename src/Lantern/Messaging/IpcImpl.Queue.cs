using Lantern.Windows;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace Lantern.Messaging;

internal partial class IpcImpl
{
    private const int MaxQueueLength = 500;
    private readonly Queue<Invocation> _queue = new();
    private readonly IAppLifetime _lifetime;
    private readonly Thread _thread;
    private bool _isAddingCompleted;

    public void Initialize()
    {
        _lifetime.ApplicationStopping.Register(() => Dispose());
        _thread.Start();
    }

    private bool Enqueue(Invocation item)
    {
        lock (_queue)
        {
            while (_queue.Count >= MaxQueueLength && !_isAddingCompleted)
            {
                Monitor.Wait(_queue);
            }

            if (!_isAddingCompleted)
            {
                Debug.Assert(_queue.Count < MaxQueueLength);
                bool startedEmpty = _queue.Count == 0;

                _queue.Enqueue(item);

                if (startedEmpty)
                {
                    // pulse for wait in Dequeue
                    Monitor.PulseAll(_queue);
                }

                return true;
            }
        }

        return false;
    }

    private bool TryDequeue(out Invocation item)
    {
        lock (_queue)
        {
            while (_queue.Count == 0 && !_isAddingCompleted)
            {
                Monitor.Wait(_queue);
            }

            if (_queue.Count > 0 && !_isAddingCompleted)
            {
                item = _queue.Dequeue();
                if (_queue.Count == MaxQueueLength - 1)
                {
                    // pulse for wait in Enqueue
                    Monitor.PulseAll(_queue);
                }

                return true;
            }

            item = default;
            return false;
        }
    }

    public void Dispose()
    {
        if (_isAddingCompleted)
            return;

        CompleteAdding();

        try
        {
            _thread.Join(1000);
        }
        catch (ThreadStateException) { }
    }

    private void CompleteAdding()
    {
        lock (_queue)
        {
            _isAddingCompleted = true;
            Monitor.PulseAll(_queue);
        }
    }

    private void RunLoop(object? state)
    {
        while (TryDequeue(out Invocation invocation))
        {
            try
            {
                HandleIpcMessage(invocation.Window, invocation.Request).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        _logger.LogError(t.Exception, "Catch exception on IPC message handling.");
                    }
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Catch exception on IPC message handling.");
            }
        }
    }

    public override void Post(IWebViewWindow window, string request) => Enqueue(new Invocation(window, request));


    private async Task HandleIpcMessage(IWebViewWindow webViewWindow, string commandAsJson)
    {
        if (webViewWindow is not WebViewWindow window)
            return;

        if (window.WindowClosed.IsCancellationRequested)
            return;

        if (commandAsJson == null)
        {
            _logger.LogWarning($"Ipc('{window.Name}') -> Request cannot be null");
            ThrowHelper.ThrowIpcRequestEmptyNameException();
        }

        _logger.LogTrace($"Ipc('{window.Name}') -> Receive request\r\n{commandAsJson}");


        var jsonOptions = _ipcOptions.JsonSerializerOptions;

        var request = JsonSerializer.Deserialize<IpcRequest>(commandAsJson, jsonOptions);

        Debug.Assert(request != null);

        string? error = null;
        object? result = null;
        IWebViewWindow? target = null;

        if (string.IsNullOrEmpty(request.Name))
        {
            error = "Request name cannot be null or emptry.";
            _logger.LogWarning($"Ipc('{window.Name}') -> {error}");
        }

        if (!_ipcOptions.TryGetBodyType(request.Name, out Type? bodyType))
        {
            error = $"Request '{request.Name}' unregisted";
            _logger.LogWarning($"Ipc('{window.Name}') -> {error}");
            goto send;
        }

        var hasBody = request.HasBody();
        if (bodyType != null && !hasBody)
        {
            error = $"Request '{request.Name}' miss body";
            _logger.LogWarning($"Ipc('{window.Name}') -> {error}");
            goto send;
        }
        else if (bodyType == null && hasBody)
        {
            error = $"Request '{request.Name}' body invaild";
            _logger.LogWarning($"Ipc('{window.Name}') -> {error}");
            goto send;
        }

        object? body = null;
        if (bodyType != null)
        {
            try
            {
                body = request.Body.Deserialize(bodyType, jsonOptions)!;
            }
            catch (Exception ex)
            {
                error = $"Request body cannot be deserialized.";
                _logger.LogWarning(ex, $"Ipc('{window.Name}') -> {error}");
                goto send;
            }
        }

        if (request.Target != null)
        {
            target = window.WindowManager?.GetWindow(request.Target);
            if (target == null)
            {
                error = $"Request '{request.Name}' window notfound '{request.Target}'";
                _logger.LogWarning($"Ipc('{window.Name}') -> request '{request.Name}' window notfound '{request.Target}'");
                goto send;
            }
        }
        else
        {
            target = window;
        }

        IpcContext context = bodyType == null ?
            new IpcContextImpl(window, target, this) :
            (IpcContext)Activator.CreateInstance(typeof(IpcContextImpl<>).MakeGenericType(bodyType), window, target, this, body)!;

        try
        {
            result = await _call.InvokeAsync<object>(request.Name, context, window.WindowClosed);

            if (result is JsonElement jsonElement &&
                (jsonElement.ValueKind == JsonValueKind.Null || jsonElement.ValueKind == JsonValueKind.Undefined))
            {
                result = null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Ipc('{window.Name}') -> '{request.Name}' handle exception\r\n{ex.Message}");
            error = ex.Message;
            goto send;
        }

        if (!request.InvoicatonId.HasValue)
        {
            return;
        }

    send:

        var hasError = error != null;
        try
        {
            var json = JsonSerializer.Serialize(new IpcResponse
            {
                InvoicatonId = request.InvoicatonId,
                Body = hasError ? error : result,
                Error = hasError,
            }, jsonOptions);

            window.PostMessage(json);

            _logger.LogTrace($"Ipc('{window.Name}') -> send response\r\n{json}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ipc('{window.Name}') -> send response error");
        }
    }

    private readonly struct Invocation
    {
        public Invocation(IWebViewWindow window,string request)
        {
            Window = window;
            Request = request;
        }

        public readonly IWebViewWindow Window;
        public readonly string Request;
    }

}
