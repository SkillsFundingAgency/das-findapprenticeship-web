using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Reqnroll.Autofac;
using SFA.DAS.FAA.AcceptanceTests.Pages;
using SFA.DAS.FAA.AcceptanceTests.Steps;

namespace SFA.DAS.FAA.AcceptanceTests;

public static class TestStartup
{
    [ScenarioDependencies]
    public static void CreateServices(ContainerBuilder builder)
    {
        builder.RegisterConfiguration();
        builder.RegisterAppSettings();
        builder.RegisterPlaywright();
        builder.RegisterPages();
        builder.RegisterType<TestContext>().As<ITestContext>().InstancePerLifetimeScope();
        builder.RegisterSteps();
    }

    private static void RegisterPages(this ContainerBuilder builder)
    {
        builder.RegisterType<HomePage>().AsSelf().InstancePerLifetimeScope();
        builder.RegisterType<SearchResultsPage>().AsSelf().InstancePerLifetimeScope();
    }
    
    private static void RegisterSteps(this ContainerBuilder builder)
    {
        builder.RegisterType<SearchStepDefinitions>().InstancePerDependency();
    }

    private static void RegisterConfiguration(this ContainerBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        builder.RegisterInstance(configuration)
            .As<IConfiguration>()
            .SingleInstance();
    }

    private static void RegisterAppSettings(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var configuration = c.Resolve<IConfiguration>();
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            return Options.Create(appSettings);
        }).As<IOptions<AppSettings>>();
    }

    private static void RegisterPlaywright(this ContainerBuilder builder)
    {
        builder.Register(async ctx =>
        {
            var appSettings = ctx.Resolve<IOptions<AppSettings>>();
            var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = appSettings.Value.Headless,
                SlowMo = appSettings.Value.SlowMo,
            }).ConfigureAwait(false);
            return await browser.NewPageAsync().ConfigureAwait(false);
        }).As<Task<IPage>>().InstancePerDependency();
    }
}