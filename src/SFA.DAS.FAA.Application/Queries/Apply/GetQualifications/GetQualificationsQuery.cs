using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetQualifications
{
    public class GetQualificationsQuery : IRequest<GetQualificationsQueryResult>
    {
        public Guid CandidateId { get; init; }
        public Guid ApplicationId { get; init; }
    }

    public class GetQualificationsQueryResult
    {
        public bool? IsSectionCompleted { get; set; }

        public List<Qualification> Qualifications { get; set; } = new List<Qualification>();

        public static implicit operator GetQualificationsQueryResult(GetQualificationsApiResponse source)
        {
            return new GetQualificationsQueryResult
            {
                IsSectionCompleted = source.IsSectionCompleted,
                Qualifications = source.Qualifications.Select(x => (Qualification)x).ToList()
            };
        }

        public class Qualification
        {
            public Guid QualificationReferenceId { get; set; }
            public string QualificationType { get; set; }
            public string Subject { get; set; }
            public string Level { get; set; }
            public string Grade { get; set; }
            public string AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }

            public static implicit operator Qualification(GetQualificationsApiResponse.Qualification source)
            {
                return new Qualification
                {
                    QualificationReferenceId = source.QualificationReferenceId,
                    QualificationType = source.QualificationType,
                    AdditionalInformation = source.AdditionalInformation,
                    IsPredicted = source.IsPredicted,
                    Grade = source.Grade,
                    Level = source.Level,
                    Subject = source.Subject
                };
            }
        }
    }

    public class GetQualificationsQueryHandler(IApiClient apiClient) : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResult>
    {
        public async Task<GetQualificationsQueryResult> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetQualificationsApiResponse>(
                new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId));

            return response;
        }
    }
}
