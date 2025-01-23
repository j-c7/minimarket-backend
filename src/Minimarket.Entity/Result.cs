namespace Minimarket.Entity;

public class Result(bool isSuccess, IEnumerable<string> errors)
{
    public bool IsSucess { get; init; } = isSuccess;

    public string[] Errors { get; init; } = errors.ToArray();

    public static Result Success() => new(true, []);
    
    public static Result Failure(IEnumerable<string> errors) => new(false, errors);
}

public class Result<T>(bool isSuccess, T? value, IEnumerable<string> errors)
{
    public bool IsSucess { get; init; } = isSuccess;

    public T? Value { get; init; } = value;

    public string[] Errors { get; init; } = errors.ToArray();

    public static Result<T> Success(T value) => new(true, value, Array.Empty<string>());
    public static Result<T> Failure(IEnumerable<string> errors) => new(false, default, errors);
}