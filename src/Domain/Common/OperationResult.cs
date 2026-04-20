namespace Domain.Common
{
    public class OperationResult<T>
    {
        public T? Value { get; private set; }
        public OperationResultStatus Status { get; private set; }
        public string? ErrorMessage { get; private set; }

        public bool IsSuccess => Status == OperationResultStatus.Success;

        public static OperationResult<T> Success(T value) =>
            new() { Value = value, Status = OperationResultStatus.Success };

        public static OperationResult<T> Failure(string errorMessage, OperationResultStatus status = OperationResultStatus.Error) =>
            new() { ErrorMessage = errorMessage, Status = status };
    }
}
