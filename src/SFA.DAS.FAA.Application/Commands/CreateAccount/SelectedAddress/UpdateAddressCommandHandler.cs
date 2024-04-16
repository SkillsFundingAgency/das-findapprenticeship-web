using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.SelectedAddress;
public class UpdateAddressCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdateAddressCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var addressLine1 = string.Empty;
        if (string.IsNullOrWhiteSpace(request.AddressLine1))
        {
            addressLine1 = !string.IsNullOrWhiteSpace(request.Thoroughfare) ? request.Thoroughfare : request.Organisation;
        }
        else
        {
            addressLine1 = request.AddressLine1;
        }

        var postRequest = new CreateUserAddressApiRequest(request.CandidateId, new CreateUserAddressApiRequestData
        {
            Uprn = request.Uprn,
            Email = request.Email,
            AddressLine1 = addressLine1,
            AddressLine2 = request.AddressLine2,
            AddressLine3 = request.AddressLine3,
            AddressLine4 = request.AddressLine4,
            Postcode = request.Postcode
        });

        await apiClient.PostWithResponseCode<NullResponse>(postRequest);

        return Unit.Value;
    }
}
