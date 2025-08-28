namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class SearchApprenticeshipFilterChoices
{
    public ChecklistDetails ApprenticeshipTypesChecklistDetails { get; set; } = new();
    public ChecklistDetails JobCategoryChecklistDetails { get; set; } = new();
    public ChecklistDetails CourseLevelsChecklistDetails { get; set; } = new();
}