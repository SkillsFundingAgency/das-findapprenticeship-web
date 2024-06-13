using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply.Base
{
    public class EqualityQuestionEthnicSubGroupViewModelBase : ViewModelBase
    {
        public Guid? ApplicationId { get; init; }
        public string? EthnicSubGroup { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; init; }
    }
}
