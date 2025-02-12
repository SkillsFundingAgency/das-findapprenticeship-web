using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteQualifications;

public class GetDeleteQualificationsQueryResult
{
    public string? QualificationReference { get; set; }

    public List<Qualification> Qualifications { get; set; } = [];

    public class Qualification
    {
        public string Subject { get; set; }
        public string Level { get; set; }
        public string Grade { get; set; }
        public string AdditionalInformation { get; set; }
        public bool? IsPredicted { get; set; }
    }

    public static implicit operator GetDeleteQualificationsQueryResult(GetDeleteQualificationsApiResponse source)
    {
        return source is null 
            ? null 
            : new GetDeleteQualificationsQueryResult
        {
            QualificationReference = source.QualificationReference,
            Qualifications = source.Qualifications.Select(x => new Qualification
            {
                Subject = x.Subject,
                Level = x.Level,
                Grade = x.Grade,
                AdditionalInformation = x.AdditionalInformation,
                IsPredicted = x.IsPredicted
            }).ToList()
        };
    }
}