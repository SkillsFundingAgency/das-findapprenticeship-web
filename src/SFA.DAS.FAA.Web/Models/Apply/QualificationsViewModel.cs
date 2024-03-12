using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class QualificationsViewModel
    {
        [FromRoute]
        public required Guid ApplicationId { get; set; }

        public bool? DoYouWantToAddAnyQualifications { get; set; }
    }

    public class AddQualificationSelectTypeViewModel
    {
        [FromRoute]
        public required Guid ApplicationId { get; set; }
    }
}
