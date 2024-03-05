namespace SFA.DAS.FAA.Web.AcceptanceTests.Data;

public static class Pages
{
    public static Page GetPage(string pageName, string applicationId)
    {
        var page = GetPages().Single(x => x.Name.Equals(pageName, StringComparison.InvariantCultureIgnoreCase));
        page.Url = page.Url.Replace(Constants.ApplicationIdKey, applicationId);
        return page;
    }

    public static List<Page> GetPages()
    {
        return
        [
            new Page { Name = "Jobs", Url = "/apply/{applicationId}/jobs" },
            new Page { Name = "Add a Job", Url = "/apply/{applicationId}/jobs/add" }
        ];
    }

}