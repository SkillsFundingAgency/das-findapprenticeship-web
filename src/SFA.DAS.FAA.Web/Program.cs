using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.ModelBinding;
using SFA.DAS.FAA.Web.Filters;

var builder = WebApplication.CreateBuilder(args);

var isIntegrationTest = builder.Environment.EnvironmentName.Equals("IntegrationTest", StringComparison.CurrentCultureIgnoreCase);
var isLocal = builder.Environment.EnvironmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);
var rootConfiguration = builder.Configuration.LoadConfiguration(isIntegrationTest);

builder.Services
    .AddOptions()
    .AddMemoryCache()
.AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new MonthYearDateModelBinderProvider());
        options.ModelBinderProviders.Insert(0, new DayMonthYearDateModelBinderProvider());
    });

builder.Services.AddConfigurationOptions(rootConfiguration);

builder.Services.AddLogging();
builder.Services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

builder.Services.AddServiceRegistration(isIntegrationTest);
builder.Services.AddAuthenticationServices(rootConfiguration);
builder.Services.AddCacheServices(rootConfiguration);
builder.Services.AddHealthChecks();



builder.Services.Configure<RouteOptions>(options =>
{

}).AddMvc(options =>
{
    //options.Filters.Add(new GoogleAnalyticsFilter());
    if (!isIntegrationTest)
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }

    options.Filters.Add(new SignInLinkFilter());

    options.Filters.Add(new NewFaaUserAccountFilter());
});

builder.Services.AddDataProtection(rootConfiguration);
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error/500");
}

app.UseHealthChecks("/ping");

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoints(endpointBuilder =>
{
    endpointBuilder.MapControllerRoute(
        name: "default",
        pattern: "{controller=SearchApprenticeshipsController}/{action=Index}/{id?}");
});

app.UseStatusCodePages();
app.Use(async (context, next) =>
{
    await next();
    if (context.Response is { StatusCode: 404, HasStarted: false })
    {
        //Re-execute the request so the user gets the error page
        var originalPath = context.Request.Path.Value;
        context.Items["originalPath"] = originalPath;
        context.Request.Path = $"/error/{context.Response.StatusCode}";
        await next();
    }
});

app.Run();
