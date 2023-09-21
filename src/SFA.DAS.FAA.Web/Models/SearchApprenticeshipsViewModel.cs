namespace SFA.DAS.FAA.Web.Models;

public class SearchApprenticeshipsViewModel
{
    public int Total { get; set; }
    public string FormattedTotal => Total.ToString("N0");


}