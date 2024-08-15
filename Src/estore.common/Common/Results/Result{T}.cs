namespace estore.common.Common.Results;

using System.Net;

public class Result<T>(bool isSuccess, T value, string errorMessage, HttpStatusCode statusCode) where T : class
{
    public bool IsSuccess { get; } = isSuccess;
    public T Value { get; } = value;
    public string ErrorMessage { get; } = errorMessage;
    public HttpStatusCode StatusCode { get; } = statusCode;

    public static Result<T> SuccessResult(T value) => new(true, value, null, HttpStatusCode.OK);

    public static Result<T> FailedResult(string errorMessage, HttpStatusCode statusCode)
        => new(false, null, errorMessage, statusCode);
}
