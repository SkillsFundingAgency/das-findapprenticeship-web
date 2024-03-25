using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply
{
    [TestFixture]
    public class QualificationsViewModelTests
    {
        [Test, MoqAutoData]
        public void Multiple_Qualifications_Of_A_Type_Are_Grouped_Together(Guid qualificationTypeId)
        {
            var fixture = new Fixture();
            var source = new GetQualificationsApiResponse
            {
                QualificationTypes =
                [
                    new GetQualificationsApiResponse.QualificationType
                    {
                        Id = qualificationTypeId,
                        Name = "gcse"
                    }
                ],
                Qualifications = fixture.CreateMany<GetQualificationsApiResponse.Qualification>().ToList()
            };

            foreach (var qualification in source.Qualifications)
            {
                qualification.QualificationReferenceId = qualificationTypeId;
            }

            var result = QualificationsViewModel.MapFromQueryResult(Guid.NewGuid(), source);

            result.QualificationGroups.Count.Should().Be(1);
        }

        [Test, MoqAutoData]
        public void Multiple_Qualifications_Of_A_Type_Are_Grouped_Separately_If_Appropriate(Guid qualificationTypeId)
        {
            var fixture = new Fixture();
            var source = new GetQualificationsApiResponse
            {
                QualificationTypes =
                [
                    new GetQualificationsApiResponse.QualificationType
                    {
                        Id = qualificationTypeId,
                        Name = "degree"
                    }
                ],
                Qualifications = fixture.CreateMany<GetQualificationsApiResponse.Qualification>().ToList()
            };

            foreach (var qualification in source.Qualifications)
            {
                qualification.QualificationReferenceId = qualificationTypeId;
            }

            var result = QualificationsViewModel.MapFromQueryResult(Guid.NewGuid(), source);

            result.QualificationGroups.Count.Should().Be(source.Qualifications.Count);
        }

    }
}
