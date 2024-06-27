using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class EqualityQuestionsEthnicSubGroupPreferNotToSaveViewModel : EqualityQuestionEthnicSubGroupViewModelBase
{
    public override EthnicGroup EthnicGroup => EthnicGroup.PreferNotToSay;
        
    public static implicit operator EqualityQuestionsEthnicSubGroupPreferNotToSaveViewModel(EqualityQuestionsEthnicGroupViewModel source)
    {
        return new EqualityQuestionsEthnicSubGroupPreferNotToSaveViewModel
        {
            ApplicationId = source.ApplicationId,
            EthnicSubGroup = null,
            OtherEthnicSubGroupAnswer = null
        };
    }
}