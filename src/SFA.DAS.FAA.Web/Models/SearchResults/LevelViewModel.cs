using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class LevelViewModel
{
    public static implicit operator LevelViewModel(LevelResponse level)
    {
        return new LevelViewModel
        {
            Selected = false,
            Name = level.Name,
            Id = level.Code
        };
    }
    public bool Selected { get; set; }
    public string Name { get; init; } = null!;
    public int Id { get; init; }
}