using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

var rootConfiguration = builder.Configuration.LoadConfiguration();

builder.Services.AddOptions();
builder.Services.Configure<FindAnApprenticeshipWebConfiguration>(rootConfiguration.GetSection(nameof(FindAnApprenticeshipWebConfiguration)));
builder.Services.AddSingleton(cfg => cfg.GetService<IOptions<FindAnApprenticeshipWebConfiguration>>()!.Value);

builder.Services.AddLogging();
builder.Services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

builder.Services.AddHealthChecks();

builder.Services.Configure<RouteOptions>(options =>
{

}).AddMvc(options =>
{
    //options.Filters.Add(new GoogleAnalyticsFilter());
    if (!rootConfiguration.IsDev())
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }

});


builder.Services.AddDataProtection(rootConfiguration);

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseHealthChecks("/ping");

app.UseRouting();

app.UseEndpoints(endpointBuilder =>
{
    endpointBuilder.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();