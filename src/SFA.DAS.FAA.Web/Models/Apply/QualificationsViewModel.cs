using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using System.Reflection;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class QualificationsViewModel
    {
        public static QualificationsViewModel MapFromQueryResult(Guid applicationId, GetQualificationsQueryResult source)
        {
            var result = new QualificationsViewModel
            {
                ApplicationId = applicationId,
                DoYouWantToAddAnyQualifications = source.Qualifications.Count == 0 && source.IsSectionCompleted is true ? false : null,
                IsSectionCompleted = source.IsSectionCompleted,
                ShowQualifications = source.Qualifications.Count != 0
            };

            foreach (var qualificationType in source.QualificationTypes.OrderBy(x => x.Order))
            {
                if (source.Qualifications.Any(x => x.QualificationReferenceId == qualificationType.Id))
                {
                    var groupMetadata = new QualificationDisplayTypeViewModel(qualificationType.Name);
                    var group = new QualificationGroup
                    {
                        DisplayName = groupMetadata.GroupTitle,
                        QualificationReferenceId = qualificationType.Id,
                        Qualifications = source.Qualifications
                            .Where(y => y.QualificationReferenceId == qualificationType.Id)
                            //.OrderBy(x => x.Order) //todo: order by 
                            .Select(z => new Qualification
                            {
                                Subject = z.Subject,
                                Grade = z.Grade,
                                Level = z.Level,
                                AdditionalInformation = z.AdditionalInformation,
                                IsPredicted = z.IsPredicted
                            }).ToList()
                    };
                    result.QualificationGroups.Add(group);
                }
            }

            return result;
        }


        [FromRoute]
        public required Guid ApplicationId { get; set; }

        public bool? DoYouWantToAddAnyQualifications { get; set; }

        public bool? IsSectionCompleted { get; set; }

        public List<QualificationGroup> QualificationGroups { get; set; } = new List<QualificationGroup>();

        public bool ShowQualifications { get; set; }

        public class QualificationGroup
        {
            public Guid QualificationReferenceId { get; set; }
            public string DisplayName { get; set; }
            public List<Qualification> Qualifications { get; set; } = new List<Qualification>();
        }

        public class Qualification
        {
            public Guid Id { get; set; }
            public string Subject { get; set; }
            public string Grade { get; set; }
            public string Level { get; set; }
            public string AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }

            public string GradeLabel => IsPredicted is true ? $"{Grade} (predicted)" : Grade;
        }
    }
}
