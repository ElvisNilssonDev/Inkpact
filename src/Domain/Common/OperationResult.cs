namespace Domain.Common
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? Error { get; private set; }
        public OperationResultStatus Status { get; private set; }

        private OperationResult() { }

        public static OperationResult<T> Success(T data) => new()
        {
            IsSuccess = true,
            Data = data,
            Status = OperationResultStatus.Ok
        };

        public static OperationResult<T> Failure(string error,
            OperationResultStatus status = OperationResultStatus.BadRequest) => new()
            {
                IsSuccess = false,
                Error = error,
                Status = status
            };

        public static OperationResult<T> NotFound(string error) =>
            Failure(error, OperationResultStatus.NotFound);

        public static OperationResult<T> Unauthorized(string error) =>
            Failure(error, OperationResultStatus.Unauthorized);

        public static OperationResult<T> Conflict(string error) =>
            Failure(error, OperationResultStatus.Conflict);
    }
}
