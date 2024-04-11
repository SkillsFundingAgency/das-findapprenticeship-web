using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsEthnicSubGroupViewModel : ViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; init; }
        public string? EthnicSubGroup { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }
    }
}
