using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.UserDateOfBirth;
public class UpdateDateOfBirthCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
}
