namespace Fiap.Banco.API.DTOs;

public record OperationResult<T>(bool Success, int StatusCode, string? Error, T? Data)
{
    public static OperationResult<T> Ok(T data, int statusCode = 200) => new(true, statusCode, null, data);
    public static OperationResult<T> Fail(string error, int statusCode) => new(false, statusCode, error, default);
}
