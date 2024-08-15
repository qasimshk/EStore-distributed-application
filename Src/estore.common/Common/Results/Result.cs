namespace estore.common.Common.Results;

using System.Net;

public class Result(bool isSuccess, List<string>? errorMessages, HttpStatusCode statusCode)
{
    public bool IsSuccess { get; } = isSuccess;
    public List<string>? ErrorMessages { get; } = errorMessages;
    public HttpStatusCode StatusCode { get; } = statusCode;

    public static Result SuccessResult() => new(true, null, HttpStatusCode.OK);

    public static Result FailedResult(List<string> errorMessages, HttpStatusCode statusCode)
        => new(false, errorMessages, statusCode);
}
