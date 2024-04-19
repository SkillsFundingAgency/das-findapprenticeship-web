using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.ManuallyEnteredAddress;
public class UpdateManuallyEnteredAddressCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdateManuallyEnteredAddressCommand, Unit>
{
    public async Task<Unit> Handle(UpdateManuallyEnteredAddressCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new CreateUserManuallyEnteredAddressApiRequest(request.CandidateId, new CreateUserManuallyEnteredAddressApiRequestData
        {
            Email = request.Email,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            TownOrCity = request.TownOrCity,
            County = request.County,
            Postcode = request.Postcode
        });

        await apiClient.PostWithResponseCode<NullResponse>(postRequest);

        return Unit.Value;
    }
}
