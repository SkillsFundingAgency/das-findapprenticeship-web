using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsEthnicGroupViewModel : ViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; init; }
        public string? EthnicGroup { get; set; }
    }
}
