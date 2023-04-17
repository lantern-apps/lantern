namespace AutoUpdates;

public class UpdateException : Exception
{
    public UpdateException(string? errorCode, string? message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public string? ErrorCode { get; set; }
}
