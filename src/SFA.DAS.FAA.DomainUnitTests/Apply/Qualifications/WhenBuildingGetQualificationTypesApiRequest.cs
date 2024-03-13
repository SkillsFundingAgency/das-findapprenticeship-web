using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.Qualifications;

public class WhenBuildingGetQualificationTypesApiRequest
{
    [Test]
    public void Then_The_Url_Is_Correctly_Built()
    {
        var actual = new GetQualificationTypesApiRequest();

        actual.GetUrl.Should().Be("referencedata/qualificationtypes");
    }
}