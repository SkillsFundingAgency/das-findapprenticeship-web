using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpsertQualification;

public class UpsertQualificationCommandHandler(IApiClient apiClient) : IRequestHandler<UpsertQualificationCommand, Unit>
{
    public async Task<Unit> Handle(UpsertQualificationCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PostWithResponseCode(new PostUpsertQualificationsApiRequest(request.ApplicationId,
            request.QualificationReferenceId,
            new PostUpsertQualificationsApiRequest.PostUpsertQualificationsApiRequestData
            {
                CandidateId = request.CandidateId,
                Subjects = request.Subjects
            }));
        return new Unit();
    }
}