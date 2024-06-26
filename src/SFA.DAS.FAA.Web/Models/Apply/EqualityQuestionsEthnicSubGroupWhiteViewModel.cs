﻿using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsEthnicSubGroupWhiteViewModel : EqualityQuestionEthnicSubGroupViewModelBase
    {
        public override EthnicGroup EthnicGroup => EthnicGroup.White;

        public static implicit operator EqualityQuestionsEthnicSubGroupWhiteViewModel(EqualityQuestionsModel source)
        {
            return new EqualityQuestionsEthnicSubGroupWhiteViewModel
            {
                ApplicationId = source.ApplicationId,
                EthnicSubGroup = source.EthnicSubGroup.StringValue(),
                OtherEthnicSubGroupAnswer = source.OtherEthnicSubGroupAnswer
            };
        }
    }
}
