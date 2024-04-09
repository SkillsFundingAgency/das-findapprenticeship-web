using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.PhoneNumber;
public class UpdatePhoneNumberCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdatePhoneNumberCommand, Unit>
{
    public async Task<Unit> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var phoneNumber = new string(request.PhoneNumber.Where(x => !char.IsWhiteSpace(x)).ToArray());
        var postRequest = new CreateUserPhoneNumberApiRequest(request.CandidateId, new CreateUserPhoneNumberApiRequestData
        {
            PhoneNumber = phoneNumber,
            Email = request.Email
        });

        await apiClient.PostWithResponseCode<NullResponse>(postRequest);

        return Unit.Value;
    }
}
