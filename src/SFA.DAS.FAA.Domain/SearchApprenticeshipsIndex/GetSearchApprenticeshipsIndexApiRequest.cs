using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

public class GetSearchApprenticeshipsIndexApiRequest : IGetApiRequest
{
    public string GetUrl => "searchapprenticeships";
}