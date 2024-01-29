using System.Security.Claims;
using SFA.DAS.FAA.Web.AppStart;

namespace SFA.DAS.FAA.Web.Extensions;

public static class ClaimsExtensions
{
    public static Guid CandidateId(this IEnumerable<Claim> claims)
    {
        if (Guid.TryParse(claims.FirstOrDefault(c => c.Type.Equals(CustomClaims.CandidateId))?.Value, out var id))
        {
            return id;    
        }

        return Guid.Empty;
    }
}