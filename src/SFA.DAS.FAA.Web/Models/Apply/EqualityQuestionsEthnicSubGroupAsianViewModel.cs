using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsEthnicSubGroupAsianViewModel : EqualityQuestionEthnicSubGroupViewModelBase
    {
        public static implicit operator EqualityQuestionsEthnicSubGroupAsianViewModel(EqualityQuestionsModel source)
        {
            return new EqualityQuestionsEthnicSubGroupAsianViewModel
            {
                ApplicationId = source.ApplicationId,
                EthnicSubGroup = source.EthnicSubGroup.StringValue(),
                OtherEthnicSubGroupAnswer = source.OtherEthnicSubGroupAnswer
            };
        }
    }
}
