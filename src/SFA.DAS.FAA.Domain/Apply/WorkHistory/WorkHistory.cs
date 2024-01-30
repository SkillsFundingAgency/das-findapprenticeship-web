using SFA.DAS.FAA.Domain.Apply.WorkHistory.Enums;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory
{
    public record WorkHistory
    {
        public Guid Id { get; set; }
        public WorkHistoryType WorkHistoryType { get; set; }
        public string? Employer { get; set; }
        public string? JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string? Description { get; set; }
    }
}
