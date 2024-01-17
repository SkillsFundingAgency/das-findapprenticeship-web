using SFA.DAS.FAA.Domain.Apply.UpsertApplication.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpsertApplication
{
    public class Application
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string? DisabilityStatus { get; set; }
        public required string VacancyReference { get; set; }
        public ApplicationStatus Status { get; set; }
        public SectionStatus IsWorkHistoryComplete { get; set; }
        public SectionStatus IsInterviewAdjustmentsComplete { get; set; }
        public SectionStatus IsEducationHistoryComplete { get; set; }
        public SectionStatus IsApplicationQuestionsComplete { get; set; }
        public SectionStatus IsDisabilityConfidenceComplete { get; set; }
    }
}