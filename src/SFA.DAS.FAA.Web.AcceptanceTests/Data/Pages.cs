namespace SFA.DAS.FAA.Web.AcceptanceTests.Data;

public static class Pages
{
    public static Page GetPage(string pageName, string applicationId)
    {
        var page = GetPages().SingleOrDefault(x => x.Name.Equals(pageName, StringComparison.InvariantCultureIgnoreCase));

        if (page == null)
        {
            throw new InvalidOperationException($"Unable to Get Page {pageName} - not found in dictionary");
        }

        page.Url = page.Url.Replace(Constants.ApplicationIdKey, applicationId);
        return page;
    }

    public static List<Page> GetPages()
    {
        return
        [
            new Page { Name = "Jobs", Url = "/apply/{applicationId}/jobs" },
            new Page { Name = "Add a Job", Url = "/apply/{applicationId}/jobs/add" },
            new Page { Name = "Application Tasklist", Url = "/applications/{applicationId}" },
            new Page { Name = "Edit Job", Url = "/apply/{applicationId}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae" },
            new Page { Name = "Delete Job", Url = "/apply/{applicationId}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae/delete" },
        ];
    }

}