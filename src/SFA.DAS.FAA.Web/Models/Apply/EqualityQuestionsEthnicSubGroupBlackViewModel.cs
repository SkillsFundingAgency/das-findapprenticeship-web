using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsEthnicSubGroupBlackViewModel : EqualityQuestionEthnicSubGroupViewModelBase
    {
        public static implicit operator EqualityQuestionsEthnicSubGroupBlackViewModel(EqualityQuestionsModel source)
        {
            return new EqualityQuestionsEthnicSubGroupBlackViewModel
            {
                ApplicationId = source.ApplicationId,
                EthnicSubGroup = source.EthnicSubGroup.StringValue(),
                OtherEthnicSubGroupAnswer = source.OtherEthnicSubGroupAnswer
            };
        }
    }
}
