using MediatR;

namespace SFA.DAS.FAA.Application.Commands.Applications.Withdraw;

public class WithdrawApplicationCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}