using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Vacancies;

public class GetSearchApprenticeshipsIndexApiRequest : IGetApiRequest
{

    public GetSearchApprenticeshipsIndexApiRequest()
    {
    }

    public string GetUrl => $"/searchApprenticeships";
}