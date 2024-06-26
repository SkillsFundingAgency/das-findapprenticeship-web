using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.Qualifications;

public class WhenBuildingGetQualificationTypesApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid applicationId, Guid candidateId)
    {
        var actual = new GetQualificationTypesApiRequest(applicationId, candidateId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/qualifications/add/select-type?candidateId={candidateId}");
    }
}