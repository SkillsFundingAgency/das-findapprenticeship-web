using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class QualificationsViewModel
    {
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
            public bool AllowMultipleAdd { get; set; }
            public bool ShowAdditionalInformation { get; set; }
            public bool? ShowLevel { get; set; }
        }

        public class Qualification
        {
            public Guid? Id { get; set; }
            public string Subject { get; set; }
            public string Grade { get; set; }
            public string AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }

            public string GradeLabel => IsPredicted is true ? $"{Grade} (predicted)" : Grade;
        }

        public static QualificationsViewModel MapFromQueryResult(Guid applicationId, GetQualificationsQueryResult source)
        {
            var result = new QualificationsViewModel
            {
                ApplicationId = applicationId,
                DoYouWantToAddAnyQualifications = source.Qualifications.Count == 0 && source.IsSectionCompleted is true ? false : null,
                IsSectionCompleted = source.IsSectionCompleted,
                ShowQualifications = source.Qualifications.Count != 0
            };

            var viewModelQualificationTypes =
                source.QualificationTypes.Select(c => new QualificationDisplayTypeViewModel(c.Name, c.Id)).ToList();

            foreach (var qualificationType in viewModelQualificationTypes.OrderBy(x => x.ListOrder))
            {
                if (source.Qualifications.All(x => x.QualificationReferenceId != qualificationType.Id))
                {
                    continue;
                }

                if (qualificationType.AllowMultipleAdd)
                {
                    var group = MapGroup(qualificationType, source.Qualifications.Where(x => x.QualificationReferenceId == qualificationType.Id));
                    result.QualificationGroups.Add(group);
                }
                else
                {
                    foreach (var qualification in source.Qualifications.Where(x => x.QualificationReferenceId == qualificationType.Id))
                    {
                        var group = MapGroup(qualificationType, new[] { qualification });
                        result.QualificationGroups.Add(group);
                    }
                }
            }

            return result;
        }

        private static QualificationGroup MapGroup(QualificationDisplayTypeViewModel qualificationType,
            IEnumerable<GetQualificationsQueryResult.Qualification> qualifications)
        {
            var result = new QualificationGroup
            {
                DisplayName = qualificationType.GroupTitle,
                ShowAdditionalInformation = qualificationType.ShouldDisplayAdditionalInformationField,
                QualificationReferenceId = qualificationType.Id,
                AllowMultipleAdd = qualificationType.AllowMultipleAdd,
                ShowLevel = qualificationType.CanShowLevel,
                Qualifications = qualifications
                    .Select(x => new Qualification
                    {
                        Id = x.Id,
                        Subject = x.Subject.Contains('|') ? x.Subject.Split('|')[1] : x.Subject,
                        Grade = x.Grade,
                        AdditionalInformation = x.AdditionalInformation,
                        IsPredicted = x.IsPredicted
                    }).ToList()
            };

            return result;
        }

    }
}
