using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.ModelBinding;
using SFA.DAS.FAA.Web.Filters;
using SFA.DAS.FAA.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var isIntegrationTest = builder.Environment.EnvironmentName.Equals("IntegrationTest", StringComparison.CurrentCultureIgnoreCase);
var rootConfiguration = builder.Configuration.LoadConfiguration(isIntegrationTest);

builder.Services
    .AddOptions()
    .AddMemoryCache()
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new MonthYearDateModelBinderProvider());
        options.ModelBinderProviders.Insert(0, new DayMonthYearDateModelBinderProvider());

        if (!isIntegrationTest)
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        }
        options.Filters.Add(new SignInLinkFilter());
        options.Filters.Add(new NewFaaUserAccountFilter());
    })
    .AddSessionStateTempDataProvider();

builder.Services.AddConfigurationOptions(rootConfiguration);

builder.Services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddServiceRegistration(isIntegrationTest);
builder.Services.AddAuthenticationServices(rootConfiguration);
builder.Services.AddCacheServices(rootConfiguration);
builder.Services.AddHealthChecks()
    .AddCheck<AzureKeyVaultSecretHealthCheck>("KeyVaultSecret", failureStatus: HealthStatus.Unhealthy)
    .AddCheck<SearchIndexStatisticsHealthCheck>("AzureSearchIndex", failureStatus: HealthStatus.Unhealthy);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();

builder.Services.AddDataProtection(rootConfiguration);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddOpenTelemetryRegistration(rootConfiguration["APPLICATIONINSIGHTS_CONNECTION_STRING"]!);

builder.Services.AddExceptionHandler<ResourceNotFoundExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error/500");
}

app.UseHealthChecks();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.AddRedirectRules();
app.UseStaticFiles();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SearchApprenticeshipsController}/{action=Index}/{id?}");

app.Run();