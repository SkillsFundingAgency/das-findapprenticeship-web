using MediatR;

namespace SFA.DAS.FAA.Application.Commands.UserDateOfBirth;
public class UpdateDateOfBirthCommand : IRequest<Unit>
{
    public string GovIdentifier { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
}
