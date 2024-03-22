using MediatR;

namespace SFA.DAS.FAA.Application.Commands.UserAddress;
public class UpdateAddressCommand : IRequest<Unit>
{
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
    public string Uprn { get; set; }
}
