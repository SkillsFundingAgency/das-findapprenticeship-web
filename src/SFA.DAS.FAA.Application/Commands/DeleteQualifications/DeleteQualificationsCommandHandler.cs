using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.DeleteQualifications
{
    public class DeleteQualificationsCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid QualificationReferenceId { get; set; }
        public Guid? Id { get; set; }
    }

    public class DeleteQualificationsCommandHandler(IApiClient apiClient) : IRequestHandler<DeleteQualificationsCommand>
    {
        public async Task Handle(DeleteQualificationsCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new PostDeleteQualificationsApiRequest(request.ApplicationId,
                request.QualificationReferenceId,
                new PostDeleteQualificationsApiRequest.PostDeleteQualificationsApiRequestBody
                {
                    CandidateId = request.CandidateId,
                    Id = request.Id
                });

            await apiClient.Post(apiRequest);
        }
    }
}
