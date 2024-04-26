using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsGenderViewModel : ViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; init; }
        public string? Sex { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }

        public static implicit operator EqualityQuestionsGenderViewModel(EqualityQuestionsModel source)
        {
            return new EqualityQuestionsGenderViewModel
            {
                ApplicationId = source.ApplicationId,
                Sex = source.Sex.StringValue(),
                IsGenderIdentifySameSexAtBirth = source.IsGenderIdentifySameSexAtBirth
            };
        }
    }
}
