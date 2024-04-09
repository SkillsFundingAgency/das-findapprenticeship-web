using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsGenderViewModel : ViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }
        public string? Sex { get; set; }
        public string? Gender { get; set; }
    }
}
