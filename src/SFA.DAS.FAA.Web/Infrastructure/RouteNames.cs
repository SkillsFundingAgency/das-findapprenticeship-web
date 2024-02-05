namespace SFA.DAS.FAA.Web.Infrastructure;

public static class RouteNames
{
    public const string ServiceStartDefault = "default";
    public const string BrowseByInterests = "browse-by-interests";
    public const string Location = "location";
    public const string SearchResults = "search-results";
    public const string Vacancies = "vacancies";
    public const string Apply = "apply";
    public const string SignOut = "sign-out";
    public const string SignedOut = "signed-out";
    public const string AccountUnavailable = "account-unavailable";
    public const string StubAccountDetailsGet = "account-details-get";
    public const string StubAccountDetailsPost = "account-details-post";
    public const string StubSignedIn = "stub-signedin-get";

    public static class ApplyApprenticeship
    {
        public const string Jobs = nameof(Jobs);
		public const string JobsSummary = nameof(JobsSummary);
        public const string EditJob = nameof(EditJob);
    }
}