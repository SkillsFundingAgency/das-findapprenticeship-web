using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident
{
    public class GetDisabilityConfidentQuery : IRequest<GetDisabilityConfidentQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }

    public class GetDisabilityConfidentQueryResult
    {
        public string EmployerName { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
    }

    public class GetDisabilityConfidentQueryHandler : IRequestHandler<GetDisabilityConfidentQuery, GetDisabilityConfidentQueryResult>
    {
        public Task<GetDisabilityConfidentQueryResult> Handle(GetDisabilityConfidentQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }


}
