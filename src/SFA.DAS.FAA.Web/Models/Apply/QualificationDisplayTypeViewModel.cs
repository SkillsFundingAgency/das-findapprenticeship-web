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
                GroupTitle = "Apprenticeships";
                Title = "Add an apprenticeship";
                ShouldDisplayAdditionalInformationField = true;
                CanShowLevel = false;
                CanShowPredicted = false;
                AllowMultipleAdd = false;
                HasDataLookup = true;
                break;
            case "btec":
                CanShowLevel = true;
                CanShowPredicted = true;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                Title = "Add a BTEC";
                GroupTitle = "BTEC";
                break;
            case "a level":
                GroupTitle = "A levels";
                Title = "Add A levels";
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = true;
                break;
            case "t level":
                GroupTitle = "T levels";
                Title = "Add a T level";
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = false;
                break;
            case "as level":
                GroupTitle = "AS levels";
                Title = "Add AS levels";
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = true;
                break;
            case "degree":
                GroupTitle = "Degree";
                Title = "Add a degree";
                CanShowLevel = false;
                CanShowPredicted = false;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                SubjectLabel = "Degree";
                AdditionalInformationLabel = "University";
                SubjectHintText = "For example, BSc Mechanical Engineering";
                break;
            case "gcse":
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = true;
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

    public string? SubjectHintText { get; set; }

    public string? AdditionalInformationLabel { get; set; }

    public string? SubjectLabel { get; set; }

    public bool CanShowLevel { get; }
    public bool CanShowPredicted { get; }
    public bool AllowMultipleAdd { get; }
    public bool ShouldDisplayAdditionalInformationField { get; set; }
    public bool HasDataLookup { get; set; }
    public string Title { get; }
    public string GroupTitle { get; }
}