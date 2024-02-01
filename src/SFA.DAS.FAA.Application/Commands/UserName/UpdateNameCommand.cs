using MediatR;

namespace SFA.DAS.FAA.Application.Commands.UserName
{
    public class UpdateNameCommand : IRequest <Unit>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string GovIdentifier { get; set; }
        public required string Email { get; set; }
    }
}
