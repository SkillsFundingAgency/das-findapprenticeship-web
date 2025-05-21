using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Applications;

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
                result.EmploymentLocation.Should().BeEquivalentTo(source.EmploymentLocation, options => options.Excluding(x => x.Addresses));
            }
        }


        [Test]
        [MoqInlineAutoData(AvailableWhere.AcrossEngland, false)]
        [MoqInlineAutoData(AvailableWhere.OneLocation, false)]
        [MoqInlineAutoData(AvailableWhere.MultipleLocations, true)]
        public void Then_EmploymentLocation_Set_Returns_Expected_Result(
            AvailableWhere locationOption,
            bool expectedResult,
            GetApplicationViewQueryResult source)
        {
            source.EmploymentLocation.EmployerLocationOption = locationOption;
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
                result.EmploymentLocation.Should().BeEquivalentTo(source.EmploymentLocation, options => options.Excluding(x => x.Addresses));
                result.ShowLocationSection.Should().Be(expectedResult);
            }
        }
    }
}
