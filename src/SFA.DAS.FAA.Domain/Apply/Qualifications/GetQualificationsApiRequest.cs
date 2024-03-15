using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetQualificationsApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/qualifications?candidateId={candidateId}";
}

public class GetQualificationsApiResponse
{
    public bool? IsSectionCompleted { get; set; }
    public List<Qualification> Qualifications { get; set; } = null!;
    public List<QualificationType> QualificationTypes { get; set; } = null!;

    public class QualificationType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public class Qualification
    {
        public string QualificationType { get; set; }
        public string Subject { get; set; }
        public string Level { get; set; }
        public string Grade { get; set; }
        public string AdditionalInformation { get; set; }
        public bool? IsPredicted { get; set; }
    }
}