using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using System.Reflection;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class QualificationsViewModel
    {
        [FromRoute]
        public required Guid ApplicationId { get; set; }

        public bool? DoYouWantToAddAnyQualifications { get; set; }

        public bool? IsSectionCompleted { get; set; }

        public List<Qualification> Qualifications { get; set; } = new List<Qualification>();

        public IEnumerable<IGrouping<string, Qualification>> QualificationGroups
        {
            get
            {
                return Qualifications.GroupBy(x => x.GroupName);
            }
        }

        public bool ShowQualifications { get; set; }

        public class Qualification
        {
            public Guid Id { get; set; }
            public string GroupName { get; set; }
            public string QualificationType { get; set; }
            public string Subject { get; set; }
            public string Grade { get; set; }
            public string Level { get; set; }
            public string AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }

            public string GradeLabel => IsPredicted is true ? $"{Grade} (predicted)" : Grade;

            public static implicit operator Qualification(GetQualificationsQueryResult.Qualification source)
            {
                return new Qualification
                {
                    GroupName = source.GroupName,
                    QualificationType = source.QualificationType,
                    Subject = source.Subject,
                    Grade = source.Grade,
                    Level = source.Level,
                    AdditionalInformation = source.AdditionalInformation,
                    IsPredicted = source.IsPredicted
                };
            }

        }
    }
}
