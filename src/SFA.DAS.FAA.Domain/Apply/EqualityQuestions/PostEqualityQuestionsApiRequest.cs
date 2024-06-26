﻿using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.EqualityQuestions;
public record PostEqualityQuestionsApiRequest(Guid CandidateId, UpdateEqualityQuestionsModel UpdateEqualityQuestionsModel)
    : IPostApiRequest
{
    public object Data { get; set; } = UpdateEqualityQuestionsModel;

    public string PostUrl => $"equalityQuestions?candidateId={CandidateId}";
}

public class UpdateEqualityQuestionsModel
{
    public GenderIdentity? Sex { get; set; }
    public EthnicGroup? EthnicGroup { get; set; }
    public EthnicSubGroup? EthnicSubGroup { get; set; }
    public string? IsGenderIdentifySameSexAtBirth { get; set; }
    public string? OtherEthnicSubGroupAnswer { get; set; }
}
