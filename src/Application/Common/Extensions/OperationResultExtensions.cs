using Domain.Common;

namespace Application.Common.Extensions
{
    public static class OperationResultExtensions
    {
        public static OperationResult<T> AsSuccess<T>(this T value) =>
            OperationResult<T>.Success(value);

        public static OperationResult<T> AsFailure<T>(this string errorMessage, OperationResultStatus status = OperationResultStatus.BadRequest) =>
            OperationResult<T>.Failure(errorMessage, status);
    }
}
