using Reqnroll;
using SFA.DAS.FAA.Web.AcceptanceTests.Data;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

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

            var url = page.Url;

            if (context.ContainsKey(ContextKeys.ApplicationId))
            {
                url = url.Replace(Constants.ApplicationIdKey, context.Get<string>(ContextKeys.ApplicationId));
            }

            if (context.ContainsKey(ContextKeys.VacancyReference))
            {
                url = url.Replace(Constants.VacancyReferenceKey, context.Get<string>(ContextKeys.VacancyReference));
            }

            page.Url = url;
            
            return page;
        }
        
        public static Responses.Response GetStatusCode(this ScenarioContext context, string responseName)
        {
            var page = Responses.GetResponses().SingleOrDefault(x => x.Name.Equals(responseName, StringComparison.InvariantCultureIgnoreCase));

            if (page == null)
            {
                throw new InvalidOperationException($"Unable to Get Response {responseName} - not found in dictionary");
            }

            return page;
        }
    }
}
