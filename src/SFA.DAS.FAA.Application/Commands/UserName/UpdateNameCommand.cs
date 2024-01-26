using MediatR;

namespace SFA.DAS.FAA.Application.Commands.UserName
{
    public class UpdateNameCommand : IRequest <Unit>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
