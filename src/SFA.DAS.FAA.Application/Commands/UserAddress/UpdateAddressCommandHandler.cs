using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.UserAddress;
public class UpdateAddressCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdateAddressCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var addressLine1 = string.Empty;
        if (String.IsNullOrWhiteSpace(request.AddressLine1))
        {
            addressLine1 = !String.IsNullOrWhiteSpace(request.Thoroughfare) ? request.Thoroughfare : request.Organisation;
        }

        var postRequest = new CreateUserAddressApiRequest(request.GovUkIdentifier, new CreateUserAddressApiRequestData
        {
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
