using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.ManuallyEnteredAddress;
public class UpdateManuallyEnteredAddressCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string TownOrCity { get; set; }
    public string? County { get; set; }
    public string Postcode { get; set; }
}
