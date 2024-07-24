namespace SFA.DAS.FAA.Web.Models
{
    public class CookiesViewModel
    {
        public required string PreviousPageUrl { get; set; }
        public bool ConsentFunctionalCookie { get; set; }
        public bool ConsentAnalyticsCookie { get; set; }
        public bool ShowBannerMessage { get; set; }
    }
}