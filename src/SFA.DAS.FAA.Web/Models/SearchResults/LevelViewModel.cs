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
            Id = level.Id
        };
    }
    public bool Selected { get; set; }
    public string Name { get; set; } = null!;
    public int Id { get; set; }
}