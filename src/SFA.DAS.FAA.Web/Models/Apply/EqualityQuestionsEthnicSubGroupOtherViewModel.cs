using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsEthnicSubGroupOtherViewModel : EqualityQuestionEthnicSubGroupViewModelBase
    {
        public override EthnicGroup EthnicGroup => EthnicGroup.Other;

        public static implicit operator EqualityQuestionsEthnicSubGroupOtherViewModel(EqualityQuestionsModel source)
        {
            return new EqualityQuestionsEthnicSubGroupOtherViewModel
            {
                ApplicationId = source.ApplicationId,
                EthnicSubGroup = source.EthnicSubGroup.StringValue(),
                OtherEthnicSubGroupAnswer = source.OtherEthnicSubGroupAnswer
            };
        }
    }
}
