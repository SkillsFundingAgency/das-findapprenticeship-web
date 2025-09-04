namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class ApprenticeshipTypesViewModel
{
    public required string HintText { get; set; }
    public required int Id { get; set; }
    public required string Name { get; set; }
    public bool Selected { get; set; }
    public required string Value { get; set; }
}