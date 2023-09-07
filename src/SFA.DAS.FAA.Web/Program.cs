using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var rootConfiguration = builder.Configuration.LoadConfiguration();

builder.Services.AddOptions();
builder.Services.Configure<FindAnApprenticeshipConfiguration>(rootConfiguration.GetSection(nameof(FindAnApprenticeshipConfiguration)));
builder.Services.AddSingleton(cfg => cfg.GetService<IOptions<FindAnApprenticeshipConfiguration>>().Value);

builder.Services.AddServiceRegistration();
builder.Services.AddAuthenticationServices();

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
        pattern: "{controller=Service}/{action=Index}/{id?}");
});

app.Run();