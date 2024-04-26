using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenMappingGetApplicationSummaryQueryResultToModel
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(GetApplicationSummaryQueryResult source)
    {
        var result = (ApplicationSummaryViewModel)source;

        using (new AssertionScope())
        {
            result.Candidate.Should().BeEquivalentTo(source.Candidate);
            result.WorkHistory.Should().BeEquivalentTo(source.WorkHistory, options => options
                    .Excluding(fil => fil.Jobs)
                    .Excluding(fil => fil.VolunteeringAndWorkExperiences));
            result.WorkHistory.Jobs.Should().BeEquivalentTo(source.WorkHistory.Jobs, options => options
                .Excluding(fil => fil!.StartDate)
                .Excluding(fil => fil!.EndDate));
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
            result.DisabilityConfidence.Should().BeEquivalentTo(source.DisabilityConfidence);
        }
    }
}