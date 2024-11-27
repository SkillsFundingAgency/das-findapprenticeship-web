using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

public class GetSearchApprenticeshipsIndexQuery : IRequest<GetSearchApprenticeshipsIndexResult>
{
    public string? LocationSearchTerm { get; set; }
    public Guid? CandidateId { get; set; }
}
