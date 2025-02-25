using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Configuration;
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
        services.AddHttpClient<IApiClient, ApiClient>()
            .ConfigureHttpClient((serviceProvider,client) =>
            {
                var findAnApprenticeshipOuterApiConfiguration = serviceProvider.GetService<IOptions<FindAnApprenticeshipOuterApi>>()!.Value;
                client.BaseAddress = !string.IsNullOrEmpty(findAnApprenticeshipOuterApiConfiguration.BaseUrlSecure) && findAnApprenticeshipOuterApiConfiguration.UseSecureGateway 
                    ? new Uri(findAnApprenticeshipOuterApiConfiguration!.BaseUrlSecure!) : new Uri(findAnApprenticeshipOuterApiConfiguration!.BaseUrl!);
            })
            .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
            {
                var findAnApprenticeshipOuterApiConfiguration = serviceProvider.GetService<IOptions<FindAnApprenticeshipOuterApi>>()!.Value;
                var logger = serviceProvider.GetService<ILogger<FindAnApprenticeshipOuterApi>>();
                if (string.IsNullOrEmpty(findAnApprenticeshipOuterApiConfiguration.SecretClientUrl))
                {
                    logger!.LogInformation("No client cert configuration to add");
                    return new HttpClientHandler();
                }

                try
                {
                    var credential = new DefaultAzureCredential();
                    var secretClient = new SecretClient(new Uri(findAnApprenticeshipOuterApiConfiguration.SecretClientUrl!), credential);
                
                    var secret = secretClient.GetSecret(findAnApprenticeshipOuterApiConfiguration.SecretName!);

                    if (!secret.HasValue)
                    {
                        throw new Exception($"Has errored - {secret.GetRawResponse().Content.ToDynamicFromJson()}");
                    }
                    
                    var handler = new HttpClientHandler();
                    handler.ClientCertificates.Add(new X509Certificate2(Convert.FromBase64String(secret.Value.Value)));
                    logger!.LogInformation("Added client cert configuration");
                    return handler;
                }
                catch (Exception e)
                {
                    logger!.LogError(e,"Unable to add client cert configuration");
                }
                return new HttpClientHandler();
            });
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