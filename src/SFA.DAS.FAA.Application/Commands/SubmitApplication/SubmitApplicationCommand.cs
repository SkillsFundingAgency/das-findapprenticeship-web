using MediatR;

namespace SFA.DAS.FAA.Application.Commands.SubmitApplication;

public class SubmitApplicationCommand : IRequest<Unit>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}