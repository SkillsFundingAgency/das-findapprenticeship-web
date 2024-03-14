namespace SFA.DAS.FAA.Web.Models.Apply;

public class QualificationDisplayTypeViewModel
{
    public QualificationDisplayTypeViewModel()
    {
    }
    public QualificationDisplayTypeViewModel(string qualificationType)
    {
        switch (qualificationType.ToLower())
        {
            case "btec":
                CanShowLevel = true;
                CanShowPredicted = true;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                Title = "Add a BTEC";
                GroupTitle = "BTEC";
                break;
            default:
                Title = "Add other qualifications";
                GroupTitle = "Other qualifications";
                CanShowLevel = false;
                CanShowPredicted = false;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                break;
        }
    }
    
    public bool CanShowLevel { get; }
    public bool CanShowPredicted { get; }
    public bool AllowMultipleAdd { get; }
    public bool ShouldDisplayAdditionalInformationField { get; set; }
    public string Title { get; }
    public string GroupTitle { get; }
    public string PluralisedTitle { get; set; }
}