using System.ComponentModel.DataAnnotations;
using SFA.DAS.FAA.Domain.Apply.UpsertApplication.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpsertApplication
{
    public class ApplicationRequest
    {
        [Required] public required string Email { get; set; }

        public ApplicationStatus Status { get; set; }
        public string? DisabilityStatus { get; set; }
        public SectionStatus IsApplicationQuestionsComplete { get; set; }
        public SectionStatus IsDisabilityConfidenceComplete { get; set; }
        public SectionStatus IsEducationHistoryComplete { get; set; }
        public SectionStatus IsInterviewAdjustmentsComplete { get; set; }
        public SectionStatus IsWorkHistoryComplete { get; set; }
    }
}