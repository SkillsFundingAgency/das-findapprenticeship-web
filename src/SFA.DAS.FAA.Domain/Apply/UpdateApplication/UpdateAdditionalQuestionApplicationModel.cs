using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;

public class UpdateAdditionalQuestionApplicationModel
{
    public SectionStatus AdditionalQuestionOne { get; set; }
    public SectionStatus AdditionalQuestionTwo { get; set; }
}