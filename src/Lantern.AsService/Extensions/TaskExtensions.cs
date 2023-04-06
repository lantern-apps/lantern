namespace Lantern.AsService;

internal static class TaskExtensions
{
    public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<bool>();
        using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s!).TrySetResult(true), tcs))
        {
            if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                throw new OperationCanceledException(cancellationToken);
        }
        await task; // already completed; propagate any exception
    }

    public static async Task WithCancellation(this Task task, TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (timeout <= TimeSpan.Zero)
        {
            await WithCancellation(task, cancellationToken).ConfigureAwait(false);
            return;
        }

        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellationTimoutSource = new CancellationTokenSource(timeout);
        using (cancellationTimoutSource.Token.Register(s => ((TaskCompletionSource<bool>)s!).TrySetResult(true), tcs))
        using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s!).TrySetResult(false), tcs))
        {
            if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
            {
                task.IgnoreException();
                throw tcs.Task.Result ? new TimeoutException() : new OperationCanceledException(cancellationToken);
            }
        }

        await task; // already completed; propagate any exception
    }

    public static void IgnoreException(this Task task)
    {
        _ = task.ContinueWith(t => t.Exception?.Handle(_ => true), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
    }
}
