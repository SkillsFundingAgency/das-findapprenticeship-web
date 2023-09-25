using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models;

public class BrowseByInterestViewModel : ViewModelBase
{
    [Required(ErrorMessage = "Select at least one job catagory you're interested in")]
    public List<string> SelectedInterests { get; set; }

    public string SelectedInterestsError => GetErrorMessage(nameof(SelectedInterests));

}