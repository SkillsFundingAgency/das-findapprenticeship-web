using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Exceptions;
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
                Qualifications = source.Qualifications.Select(x => (Qualification)x).OrderBy(fil => fil.QualificationOrder).ToList(),
                QualificationTypes = source.QualificationTypes.Select(x => (QualificationType)x).ToList()
            };
        }

        public List<QualificationType> QualificationTypes { get; set; } = null!;

        public class QualificationType
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }

            public static implicit operator QualificationType(GetQualificationsApiResponse.QualificationType source)
            {
                return new QualificationType
                {
                    Id = source.Id,
                    Name = source.Name,
                    Order = source.Order
                };
            }
        }

        public class Qualification
        {
            public Guid QualificationReferenceId { get; set; }
            public string Subject { get; set; }
            public string Grade { get; set; }
            public string AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }
            public short? QualificationOrder { get; set; }
            public Guid? Id { get; set; }

            public static implicit operator Qualification(GetQualificationsApiResponse.Qualification source)
            {
                return new Qualification
                {
                    QualificationReferenceId = source.QualificationReferenceId,
                    AdditionalInformation = source.AdditionalInformation,
                    IsPredicted = source.IsPredicted,
                    Grade = source.Grade,
                    Subject = source.Subject,
                    QualificationOrder = source.QualificationOrder,
                    Id = source.Id
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

            if (response == null) throw new ResourceNotFoundException();

            return response;
        }
    }
}
