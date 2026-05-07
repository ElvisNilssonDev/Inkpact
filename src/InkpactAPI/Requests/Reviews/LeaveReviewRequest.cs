namespace InkpactAPI.Requests.Reviews
{
    public record LeaveReviewRequest(
    Guid ContractId,
    int Rating,
    string Comment
);
}
