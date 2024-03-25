namespace SFA.DAS.FAA.Web.Models.Apply;

public class QualificationDisplayTypeViewModel
{
    public QualificationDisplayTypeViewModel()
    {
    }
    public QualificationDisplayTypeViewModel(string qualificationType, Guid id)
    {
        switch (qualificationType.ToLower())
        {
            case "apprenticeship":
                Id = id;
                GroupTitle = "Apprenticeships";
                Title = "Add an apprenticeship";
                SubjectLabel = "Training course";
                SubjectHintText = "For example, Network engineer (Level 4)";
                ShouldDisplayAdditionalInformationField = false;
                CanShowLevel = false;
                CanShowPredicted = false;
                AllowMultipleAdd = false;
                HasDataLookup = true;
                AddOrder = 6;
                ListOrder = 2;
                break;
            case "btec":
                Id = id;
                CanShowLevel = true;
                CanShowPredicted = true;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = false;
                Title = "Add a BTEC";
                GroupTitle = "BTEC";
                AddOrder = 2;
                ListOrder = 6;
                break;
            case "a level":
                Id = id;
                GroupTitle = "A levels";
                Title = "Add A levels";
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = true;
                AddOrder = 5;
                ListOrder = 3;
                break;
            case "t level":
                Id = id;
                GroupTitle = "T levels";
                Title = "Add a T level";
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = false;
                AddOrder = 3;
                ListOrder = 5;
                break;
            case "as level":
                Id = id;
                GroupTitle = "AS levels";
                Title = "Add AS levels";
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = true;
                AddOrder = 4;
                ListOrder = 4;
                break;
            case "degree":
                Id = id;
                GroupTitle = "Degree";
                Title = "Add a degree";
                CanShowLevel = false;
                CanShowPredicted = false;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                SubjectLabel = "Degree";
                AdditionalInformationLabel = "University";
                SubjectHintText = "For example, BSc Mechanical Engineering";
                SelectHintText = "(undergraduate, postgraduate or foundation)";
                AddOrder = 7;
                ListOrder = 1;
                break;
            case "gcse":
                Id = id;
                CanShowLevel = false;
                CanShowPredicted = true;
                AllowMultipleAdd = true;
                ShouldDisplayAdditionalInformationField = false;
                Title = "Add GCSEs";
                GroupTitle = "GCSEs";
                AddOrder = 1;
                ListOrder = 7;
                break;
            default:
                Id = id;
                Title = "Add other qualifications";
                GroupTitle = "Other qualifications";
                CanShowLevel = false;
                CanShowPredicted = false;
                AllowMultipleAdd = false;
                ShouldDisplayAdditionalInformationField = true;
                SelectHintText = "(including international qualifications)";
                AddOrder = 99;
                ListOrder = 99;
                break;
        }
    }
    public Guid Id { get; set; }

    public short ListOrder { get; set; }
    public short AddOrder { get; set; }
    public string? SelectHintText { get; set; }
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