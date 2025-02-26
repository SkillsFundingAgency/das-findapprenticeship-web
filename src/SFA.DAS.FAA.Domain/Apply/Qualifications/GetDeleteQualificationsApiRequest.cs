using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications
{
    public class GetDeleteQualificationsApiRequest(Guid applicationId, Guid candidateId, Guid qualificationType, Guid? id = null) : IGetApiRequest
    {
        public string GetUrl => $"applications/{applicationId}/qualifications/delete/{qualificationType}?candidateId={candidateId}&id={id}";
    }

    public class GetDeleteQualificationsApiResponse
    {
        public string QualificationReference { get; set; } = null!;
        public List<Qualification> Qualifications { get; set; } = null!;

        public class Qualification
        {
            public string Subject { get; set; }
            public string Level { get; set; }
            public string Grade { get; set; }
            public string AdditionalInformation { get; set; }
            public short? QualificationOrder { get; set; }
            public bool? IsPredicted { get; set; }
        }
    }
}
