using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsEthnicGroupViewModel : ViewModelBase
    {
        public Guid? ApplicationId { get; init; }
        public string? EthnicGroup { get; set; }

        public static implicit operator EqualityQuestionsEthnicGroupViewModel(EqualityQuestionsModel source)
        {
            return new EqualityQuestionsEthnicGroupViewModel
            {
                ApplicationId = source.ApplicationId,
                EthnicGroup = ((int)source.EthnicGroup).ToString(),
            };
        }
    }
}
