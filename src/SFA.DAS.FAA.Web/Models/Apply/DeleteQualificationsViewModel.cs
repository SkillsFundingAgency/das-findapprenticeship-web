using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteQualifications;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class DeleteQualificationsViewModel
    {
        [FromRoute] public required Guid ApplicationId { get; set; }
        [FromRoute] public required Guid QualificationReferenceId { get; set; }

        public string DisplayName { get; set; }
        public bool ShowAdditionalInformation { get; set; }

        public List<Qualification> Qualifications { get; set; } = new List<Qualification>();

        public string PageTitle => Qualifications.Count == 1 ? "Do you want to delete this qualification?" : "Do you want to delete these qualifications?";

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

        public static DeleteQualificationsViewModel MapFromQueryResult(Guid applicationId, Guid qualificationReferenceId,
            GetDeleteQualificationsQueryResult source)
        {
            var metadata = new QualificationDisplayTypeViewModel(source.QualificationReference);

            var result = new DeleteQualificationsViewModel
            {
                ApplicationId = applicationId,
                QualificationReferenceId = qualificationReferenceId,
                DisplayName = metadata.GroupTitle,
                ShowAdditionalInformation = metadata.ShouldDisplayAdditionalInformationField,
                Qualifications = source.Qualifications.Select(z => new Qualification
                {
                    Subject = z.Subject,
                    Grade = z.Grade,
                    Level = z.Level,
                    AdditionalInformation = z.AdditionalInformation,
                    IsPredicted = z.IsPredicted
                }).ToList()
            };

            return result;
        }
    }
}
