using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.Qualifications;

public class WhenBuildingGetAddQualificationApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid qualificationReferenceId, Guid applicationId)
    {
        var actual = new GetModifyQualificationApiRequest(qualificationReferenceId, applicationId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/qualifications/{qualificationReferenceId}/modify");
    }
}