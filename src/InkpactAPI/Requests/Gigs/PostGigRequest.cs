namespace InkpactAPI.Requests.Gigs
{
    public record PostGigRequest(
    string Title,
    string Description,
    decimal Budget,
    DateTime Deadline,
    string Tags
);
}
