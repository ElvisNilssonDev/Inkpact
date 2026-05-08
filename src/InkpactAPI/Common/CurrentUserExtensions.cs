using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace InkpactAPI.Common
{
    public static class CurrentUserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim))
                throw new UnauthorizedAccessException("User Id not found in token.");

            return Guid.Parse(idClaim);
        }

        public static Guid? TryGetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim))
                return null;

            return Guid.TryParse(idClaim, out var id) ? id : null;
        }
    }
}