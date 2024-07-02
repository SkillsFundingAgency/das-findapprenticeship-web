using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Apply.Base
{
    public abstract class EqualityQuestionEthnicSubGroupViewModelBase : ViewModelBase
    {
        public Guid? ApplicationId { get; init; }
        public bool IsEdit { get; set; }
        public abstract EthnicGroup EthnicGroup { get; }
        public string? EthnicSubGroup { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; init; }
    }
}
