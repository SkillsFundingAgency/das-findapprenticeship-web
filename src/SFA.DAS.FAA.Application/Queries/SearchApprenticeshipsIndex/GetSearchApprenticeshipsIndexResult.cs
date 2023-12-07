using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex
{
    public class GetSearchApprenticeshipsIndexResult
    {
        public int Total { get; set; }
        public Location? Location { get; set; }
        public bool LocationSearched { get; set; }
    }
}