using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenMappingGetApplicationSummaryQueryResultToModel
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(GetApplicationSummaryQueryResult source)
    {
        var result = (ApplicationSummaryViewModel)source;

        using (new AssertionScope())
        {
            result.Candidate.Should().BeEquivalentTo(source.Candidate, options => options.ExcludingMissingMembers());
            result.WorkHistory.Should().BeEquivalentTo(source.WorkHistory, options => options.ExcludingMissingMembers());
            result.WorkHistory.Jobs.Should().BeEquivalentTo(source.WorkHistory.Jobs, options => options.ExcludingMissingMembers());
            result.WorkHistory.VolunteeringAndWorkExperiences.Should().BeEquivalentTo(source.WorkHistory.VolunteeringAndWorkExperiences, options => options
                .Excluding(fil => fil!.StartDate)
                .Excluding(fil => fil!.EndDate)
                .Excluding(fil => fil!.JobTitle));
            result.EducationHistory.Should().BeEquivalentTo(source.EducationHistory, options => options
                .Excluding(fil => fil.Qualifications)
                .Excluding(fil => fil.QualificationTypes));
            result.InterviewAdjustments.Should().BeEquivalentTo(source.InterviewAdjustments);
            result.ApplicationQuestions.Should().BeEquivalentTo(source.ApplicationQuestions);
            result.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
            result.AboutYou.Should().BeEquivalentTo(source.AboutYou);
            result.WhatIsYourInterest.Should().BeEquivalentTo(source.WhatIsYourInterest);
            result.EmploymentLocation.Should().BeEquivalentTo(source.EmploymentLocation, options => options.Excluding(x => x.Addresses));
            result.DisabilityConfidence.Should().BeEquivalentTo(source.DisabilityConfidence);
        }
    }

    [Test]
    [MoqInlineAutoData(AvailableWhere.AcrossEngland, false)]
    [MoqInlineAutoData(AvailableWhere.OneLocation, false)]
    [MoqInlineAutoData(AvailableWhere.MultipleLocations, true)]
    public void Then_EmploymentLocation_Set_Returns_Expected_Result(
        AvailableWhere locationOption,
        bool expectedResult,
        GetApplicationSummaryQueryResult source)
    {
        source.EmploymentLocation.EmployerLocationOption = locationOption;
        var result = (ApplicationSummaryViewModel)source;

        using (new AssertionScope())
        {
            result.Candidate.Should().BeEquivalentTo(source.Candidate, options => options.ExcludingMissingMembers());
            result.WorkHistory.Should().BeEquivalentTo(source.WorkHistory, options => options.ExcludingMissingMembers());
            result.WorkHistory.Jobs.Should().BeEquivalentTo(source.WorkHistory.Jobs, options => options.ExcludingMissingMembers());
            result.WorkHistory.VolunteeringAndWorkExperiences.Should().BeEquivalentTo(source.WorkHistory.VolunteeringAndWorkExperiences, options => options
                .Excluding(fil => fil!.StartDate)
                .Excluding(fil => fil!.EndDate)
                .Excluding(fil => fil!.JobTitle));
            result.EducationHistory.Should().BeEquivalentTo(source.EducationHistory, options => options
                .Excluding(fil => fil.Qualifications)
                .Excluding(fil => fil.QualificationTypes));
            result.InterviewAdjustments.Should().BeEquivalentTo(source.InterviewAdjustments);
            result.ApplicationQuestions.Should().BeEquivalentTo(source.ApplicationQuestions);
            result.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
            result.AboutYou.Should().BeEquivalentTo(source.AboutYou);
            result.WhatIsYourInterest.Should().BeEquivalentTo(source.WhatIsYourInterest);
            result.EmploymentLocation.Should().BeEquivalentTo(source.EmploymentLocation, options => options.Excluding(x => x.Addresses));
            result.DisabilityConfidence.Should().BeEquivalentTo(source.DisabilityConfidence);
            result.ShowLocationSection.Should().Be(expectedResult);
        }
    }
}