using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.UserName
{
    public class UpdateNameCommand : IRequest<Unit>
    {
        public Guid CandidateId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }
}
