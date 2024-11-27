using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

public class GetSearchApprenticeshipsIndexApiRequest(string? location, Guid? candidateId) : IGetApiRequest
{
    public string GetUrl => $"searchapprenticeships?locationSearchTerm={HttpUtility.UrlEncode(location)}&candidateId={candidateId}";
}