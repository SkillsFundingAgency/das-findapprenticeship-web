using MediatR;

namespace SFA.DAS.FAA.Application.Commands.WorkHistory.DeleteJob;

public class PostDeleteJobCommand : IRequest<Unit>
{
    public Guid JobId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}