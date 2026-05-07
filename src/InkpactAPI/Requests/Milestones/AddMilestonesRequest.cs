namespace InkpactAPI.Requests.Milestones
{
    public record AddMilestoneRequest(
    Guid ContractId,
    string Title,
    string Description,
    decimal Amount,
    DateTime DueDate
);
}
