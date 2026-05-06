
namespace InkpactAPI.Requests.Gigs
{
    public record EditGigRequest(
    string Title,
    string Description,
    decimal Budget,
    DateTime Deadline,
    string Tags
);
}
