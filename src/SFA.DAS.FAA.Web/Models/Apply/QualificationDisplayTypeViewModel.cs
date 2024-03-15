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
            case "apprenticeship":
                GroupTitle = "Apprenticeship";
                break;
            case "btec":
                CanShowLevel = true;
                CanShowPredicted = true;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                Title = "Add a BTEC";
                GroupTitle = "BTEC";
                break;
            case "a levels":
                GroupTitle = "A levels";
                break;
            case "t levels":
                GroupTitle = "T levels";
                break;
            case "degree":
                GroupTitle = "Degree";
                break;
            case "gcse":
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                Title = "Add GCSEs";
                GroupTitle = "GCSEs";
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