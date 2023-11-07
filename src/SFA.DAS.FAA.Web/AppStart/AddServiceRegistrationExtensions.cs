using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetSearchApprenticeshipsIndexQuery).Assembly));
    }
}