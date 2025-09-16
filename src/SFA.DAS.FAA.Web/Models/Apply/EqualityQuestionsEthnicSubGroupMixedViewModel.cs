using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class EqualityQuestionsEthnicSubGroupMixedViewModel : EqualityQuestionEthnicSubGroupViewModelBase
{
    public override EthnicGroup EthnicGroup => EthnicGroup.MixedOrMultiple;

    public static implicit operator EqualityQuestionsEthnicSubGroupMixedViewModel(EqualityQuestionsModel source)
    {
        return new EqualityQuestionsEthnicSubGroupMixedViewModel
        {
            ApplicationId = source.ApplicationId,
            EthnicSubGroup = source.EthnicSubGroup.StringValue(),
            OtherEthnicSubGroupAnswer = source.OtherEthnicSubGroupAnswer
        };
    }
}