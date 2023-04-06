namespace Lantern.Aus;

public class AusException : Exception
{
    public AusException(string? errorCode, string? message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public string? ErrorCode { get; set; }
}
