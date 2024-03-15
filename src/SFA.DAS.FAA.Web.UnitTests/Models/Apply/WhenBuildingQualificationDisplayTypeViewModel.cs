using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenBuildingQualificationDisplayTypeViewModel
{
    [Test]
    public void Then_Values_Set_For_Btec()
    {
        var actual = new QualificationDisplayTypeViewModel("BTEC");

        actual.Title.Should().Be("Add a BTEC");
        actual.GroupTitle.Should().Be("BTEC");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeTrue();
        actual.ShouldDisplayAdditionalInformationField.Should().BeTrue();
    }
    [Test]
    public void Then_Values_Set_For_Other()
    {
        var actual = new QualificationDisplayTypeViewModel("MAgic");

        actual.Title.Should().Be("Add other qualifications");
        actual.GroupTitle.Should().Be("Other qualifications");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeFalse();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeTrue();
    }
}