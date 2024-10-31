namespace Hospital.Application.Infrastructure
{
    public class CqrsResult<T>
    {
        public T? Value { get; private set; }
        public bool IsSuccess { get; private set; }
        public string? Error { get; private set; }

        public static CqrsResult<T> Success(T value) => new() { Value = value, IsSuccess = true };
        public static CqrsResult<T> Failed(string error) => new() { IsSuccess = false, Error = error };
    }
}
