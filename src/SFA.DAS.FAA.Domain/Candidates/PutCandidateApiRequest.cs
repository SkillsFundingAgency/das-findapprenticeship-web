using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Candidates;
public class PutCandidateApiRequest : IPutApiRequest
{
    private readonly Guid _candidateId;
    public object Data { get; set; }

    public PutCandidateApiRequest(Guid candidateId, object data)
    {
        _candidateId = candidateId;
        Data = data;
    }

    public string PutUrl => $"candidates/{_candidateId}";
}

public class PutCandidateApiRequestData
{
    public string Email { get; set; }
    public string MobilePhone { get; set; }
}
