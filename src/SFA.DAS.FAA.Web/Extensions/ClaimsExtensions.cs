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

    public static string? Email(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
    }

    public static string GovIdentifier(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
    }

    public static string PhoneNumber(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.MobilePhone))?.Value;
    }
}