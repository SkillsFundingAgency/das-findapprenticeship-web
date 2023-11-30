using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Services;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetSearchApprenticeshipsIndexQuery).Assembly));
        services.AddTransient<IDateTimeService, DateTimeService>();
    }
}