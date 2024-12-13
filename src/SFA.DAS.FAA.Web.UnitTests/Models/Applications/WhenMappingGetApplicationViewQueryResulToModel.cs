using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Applications
{
    public class WhenMappingGetApplicationViewQueryResulToModel
    {
        [Test, MoqAutoData]
        public void Map_Returns_Expected_Result(GetApplicationViewQueryResult source)
        {
            var result = (ApplicationViewModel)source;

            using (new AssertionScope())
            {
                result.Candidate.Should().BeEquivalentTo(source.Candidate, options => options.ExcludingMissingMembers());
                result.WorkHistory.Jobs.Should().BeEquivalentTo(source.WorkHistory.Jobs, options => options.ExcludingMissingMembers());
                result.WorkHistory.VolunteeringAndWorkExperiences.Should().BeEquivalentTo(source.WorkHistory.VolunteeringAndWorkExperiences,
                    options => options.ExcludingMissingMembers());
                result.EducationHistory.Should().BeEquivalentTo(source.EducationHistory, options => options
                    .Excluding(fil => fil.Qualifications)
                    .Excluding(fil => fil.QualificationTypes));
                result.InterviewAdjustments.Should().BeEquivalentTo(source.InterviewAdjustments);
                result.ApplicationQuestions.Should().BeEquivalentTo(source.ApplicationQuestions);
                result.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
                result.AboutYou.Should().BeEquivalentTo(source.AboutYou);
                result.WhatIsYourInterest.Should().BeEquivalentTo(source.WhatIsYourInterest);
                result.DisabilityConfidence.Should().BeEquivalentTo(source.DisabilityConfidence);
                result.VacancyDetails.Should().BeEquivalentTo(source.VacancyDetails);
            }
        }
    }
}
