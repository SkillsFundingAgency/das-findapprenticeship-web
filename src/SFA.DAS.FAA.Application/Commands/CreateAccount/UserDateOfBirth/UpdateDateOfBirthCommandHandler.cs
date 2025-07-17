using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.UserDateOfBirth;
public class UpdateDateOfBirthCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdateDateOfBirthCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDateOfBirthCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new UpdateDateOfBirthApiRequest(request.CandidateId, new UpdateDateOfBirthRequestData
        {
            DateOfBirth = request.DateOfBirth,
            Email = request.Email
        });

        await apiClient.Post<NullResponse>(postRequest);

        return Unit.Value;
    }
}
