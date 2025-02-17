using System.Security.Claims;
using FluentValidation.AspNetCore;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services, bool devDecrypt)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetSearchApprenticeshipsIndexQuery).Assembly));
        services.AddTransient<IDateTimeService, DateTimeService>();
        
       
        services.AddFluentValidationAutoValidation();
        if (devDecrypt)
        {
            services.AddTransient<IDataProtectorService, DevDataProtectorService>();
        }
        else
        {
            services.AddTransient<IDataProtectorService, DataProtectorService>();
        }
    }

    public static void AddCacheServices(this IServiceCollection services,  IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(FindAnApprenticeship))
            .Get<Domain.Configuration.FindAnApprenticeship>();
        
        if (string.IsNullOrEmpty(config.RedisConnectionString))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.RedisConnectionString;
            });
        }
        services.AddTransient<ICacheStorageService, CacheStorageService>();
    }

    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var cookieDomain = DomainExtensions.GetDomain(configuration["ResourceEnvironmentName"]);
        var loginRedirect = string.IsNullOrEmpty(cookieDomain)? "" : $"https://{cookieDomain}/account-details";
        
        services.AddAndConfigureGovUkAuthentication(
            configuration,
            new AuthRedirects
            {
                SignedOutRedirectUrl = "/apprenticeshipsearch",
                LoginRedirect = loginRedirect,
                CookieDomain = cookieDomain,
                LocalStubLoginPath = "/account-details"
            },
            typeof(CandidateAccountPostAuthenticationClaimsHandler));
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
                    policy.RequireClaim(CustomClaims.CandidateId);
                    policy.RequireAuthenticatedUser();
                });
        });
    }
}