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
                ErrorSummary = "Enter your apprenticeship";
                SubjectErrorMessage = "Enter the training course you studied during your apprenticeship";
                AdditionalInformationLabel = "Company";
                AdditionalInformationErrorMessage = "Enter the company you worked for during your apprenticeship";
                GradeErrorMessage = "Select a grade";
                ShouldDisplayAdditionalInformationField = true;
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
                ErrorSummary = "Enter your BTEC";
                SubjectErrorMessage = "Enter your BTEC subject";
                AdditionalInformationErrorMessage = "Select the level of your BTEC";
                GradeErrorMessage = "Enter the grade for your BTEC";
                Title = "Add a BTEC";
                GroupTitle = "BTEC";
                AddOrder = 2;
                ListOrder = 6;
                break;
            case "a level":
                Id = id;
                GroupTitle = "A levels";
                Title = "Add A levels";
                ErrorSummary = "Enter your A level subjects and grades";
                SubjectErrorMessage = "Enter the subject for this grade";
                GradeErrorMessage = "Enter the grade for this subject";
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
                ErrorSummary = "Enter your T level";
                SubjectErrorMessage = "Enter the subject of your T level";
                GradeErrorMessage = "Select the grade of your T level";
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
                ErrorSummary = "Enter your AS level subjects and grades";
                SubjectErrorMessage = "Enter the subject for this grade";
                GradeErrorMessage = "Enter the grade for this subject";
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
                ErrorSummary = "Enter your degree";
                GradeErrorMessage = "Enter the grade you got in your degree";
                SubjectErrorMessage = "Enter the name of the degree you studied";
                AdditionalInformationErrorMessage = "Enter the university where you studied your degree";
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
                ErrorSummary = "Enter your GCSE subjects and grades";
                SubjectErrorMessage = "Enter the subject for this grade";
                GradeErrorMessage = "Enter the grade for this subject";
                Title = "Add GCSEs";
                GroupTitle = "GCSEs";
                AddOrder = 1;
                ListOrder = 7;
                break;
            default:
                Id = id;
                Title = "Add other qualifications";
                GroupTitle = "Other qualifications";
                ErrorSummary = "Enter your other qualification";
                AdditionalInformationErrorMessage = "Enter the type of the qualification you want to add";
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
    public string ErrorSummary { get; set; }
    public string? SubjectErrorMessage { get; set; } = "Error";
    public string? GradeErrorMessage { get; set; } = "Error";
    public string? AdditionalInformationErrorMessage { get; set; } = "Error";
    
}