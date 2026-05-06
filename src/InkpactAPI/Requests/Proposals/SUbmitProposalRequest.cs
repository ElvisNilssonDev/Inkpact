namespace InkpactAPI.Requests.Proposals
{
    public record SubmitProposalRequest(
    Guid GigId,
    string CoverLetter,
    decimal ProposedRate,
    int EstimatedDays
);
}
