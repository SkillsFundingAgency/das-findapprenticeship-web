using System.Security.Claims;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Services;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetSearchApprenticeshipsIndexQuery).Assembly));
        services.AddTransient<IDateTimeService, DateTimeService>();
    }

    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAndConfigureGovUkAuthentication(
            configuration,
            typeof(CandidateAccountPostAuthenticationClaimsHandler),
            "",
            "/service/account-details");
        services.AddHttpContextAccessor();
        services.AddTransient<ICustomClaims, CandidateAccountPostAuthenticationClaimsHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                PolicyNames.IsAuthenticated, policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
            options.AddPolicy(
                PolicyNames.IsFaaUser, policy =>
                {
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                    policy.RequireClaim(ClaimTypes.Email);
                    policy.RequireClaim(ClaimTypes.MobilePhone);
                    policy.RequireAuthenticatedUser();
                });
        });
    }
}