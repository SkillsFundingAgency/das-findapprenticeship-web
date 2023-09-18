using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

public class GetSearchApprenticeshipsIndexApiRequest : IGetApiRequest
{

    public GetSearchApprenticeshipsIndexApiRequest()
    {
    }

    public string GetUrl => $"/searchApprenticeships";
}