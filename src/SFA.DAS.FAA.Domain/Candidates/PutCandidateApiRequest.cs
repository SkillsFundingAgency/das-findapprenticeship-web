using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Candidates;
public class PutCandidateApiRequest : IPutApiRequest
{
    private readonly string govIdentifier;
    public object Data { get; set; }

    public PutCandidateApiRequest(string govIdentifier, PutCandidateApiRequestData data)
    {
        this.govIdentifier = govIdentifier;
        Data = data;
    }

    public string PutUrl => $"candidates/{govIdentifier}";
}

public class PutCandidateApiRequestData
{
    public string Email { get; set; }
}
