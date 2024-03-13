using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class QualificationsViewModel
    {
        [FromRoute]
        public required Guid ApplicationId { get; set; }

        public bool? DoYouWantToAddAnyQualifications { get; set; }

        public bool? IsSectionCompleted { get; set; }

        public List<Qualification> Qualifications { get; set; } = new List<Qualification>();
    
        public bool ShowQualifications { get; set; }

        public class Qualification
        {

            public static implicit operator Qualification(GetQualificationsQueryResult.Qualification source)
            {
                return new Qualification();
            }

        }
    }

    public class AddQualificationSelectTypeViewModel
    {
        [FromRoute]
        public required Guid ApplicationId { get; set; }

        public List<QualificationTypeApiResponse> Qualifications { get; set; } = [];
        
        public Guid QualificationReferenceId { get; set; }
    }
}
