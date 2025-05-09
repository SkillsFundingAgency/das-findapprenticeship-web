using SFA.DAS.FAA.Web.AppStart;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.Extensions;

public static class ClaimsExtensions
{
    public static Guid? CandidateId(this IEnumerable<Claim> claims)
    {
        return Guid.TryParse(claims.FirstOrDefault(c => c.Type.Equals(CustomClaims.CandidateId))?.Value, out var id)
            ? id
            : null;
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

    public static bool IsAccountSetupCompleted(this IEnumerable<Claim> claims)
    {
        var claimsList = claims.ToList();
        return claimsList.Any(p => p.Type == CustomClaims.CandidateId) &&
               claimsList.Any(p => p.Type == CustomClaims.AccountSetupCompleted);
    }
}