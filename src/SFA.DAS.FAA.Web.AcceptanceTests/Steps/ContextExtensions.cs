using Reqnroll;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

public static class ContextExtensions
{
    public static void ClearResponseContext(this ScenarioContext context)
    {
        context.Remove(ContextKeys.HttpResponse);
        context.Remove(ContextKeys.HttpResponseContent);
        context.Remove(ContextKeys.HttpResponseRedirectContent);
    }
}