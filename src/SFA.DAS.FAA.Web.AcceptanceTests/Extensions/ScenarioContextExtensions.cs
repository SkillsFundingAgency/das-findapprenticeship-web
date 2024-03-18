
using SFA.DAS.FAA.Web.AcceptanceTests.Data;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Extensions
{
    public static class ScenarioContextExtensions
    {
        public static Pages.Page GetPage(this ScenarioContext context, string pageName)
        {
            var page = Pages.GetPages().SingleOrDefault(x => x.Name.Equals(pageName, StringComparison.InvariantCultureIgnoreCase));

            if (page == null)
            {
                throw new InvalidOperationException($"Unable to Get Page {pageName} - not found in dictionary");
            }

            page.Url = page.Url
                .Replace(Constants.ApplicationIdKey, context.Get<string>(ContextKeys.ApplicationId))
                .Replace(Constants.VacancyReferenceKey, context.Get<string>(ContextKeys.VacancyReference));
            return page;
        }
    }
}
