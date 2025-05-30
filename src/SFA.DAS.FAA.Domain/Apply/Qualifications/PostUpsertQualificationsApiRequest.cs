using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class PostUpsertQualificationsApiRequest(Guid applicationId, Guid qualificationReferenceId, PostUpsertQualificationsApiRequest.PostUpsertQualificationsApiRequestData body) : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/qualifications/{qualificationReferenceId}/modify";
    public object Data { get; set; } = body;

    public class PostUpsertQualificationsApiRequestData
    {
        public List<Subject> Subjects { get; set; }
        public Guid CandidateId { get; set; }
    }


    public class Subject
    {
        public Guid? Id { get; set; }
        public string Grade { get; set; }
        public string Name { get; set; }
        public string? AdditionalInformation { get; set; }
        public bool? IsPredicted { get; set; }
        public short? QualificationOrder { get; set; }
        public bool? IsDeleted { get; set; }
    }
}