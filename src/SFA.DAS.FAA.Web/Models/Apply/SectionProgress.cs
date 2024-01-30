using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public record SectionProgress
    {
        public SectionStatus Id { get; init; }
        public string? StatusLabel { get; init; }
    }
}
