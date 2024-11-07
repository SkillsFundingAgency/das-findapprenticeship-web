using MediatR;
public class SaveSearchCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public bool DisabilityConfident { get; set; }
    public int? Distance { get; set; }
    public string? Location { get; set; }
    public string? SearchTerm { get; set; }
    public List<string>? SelectedLevelIds { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public string? SortOrder { get; set; }
}