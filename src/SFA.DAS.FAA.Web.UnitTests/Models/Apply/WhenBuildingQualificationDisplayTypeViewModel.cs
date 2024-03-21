using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenBuildingQualificationDisplayTypeViewModel
{
    [Test]
    public void Then_Values_Set_For_Apprenticeship()
    {
        var actual = new QualificationDisplayTypeViewModel("Apprenticeship");

        actual.Title.Should().Be("Add an apprenticeship");
        actual.GroupTitle.Should().Be("Apprenticeships");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeFalse();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeTrue();
        actual.HasDataLookup.Should().BeTrue();
    }
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
    public void Then_Values_Set_For_ALevel()
    {
        var actual = new QualificationDisplayTypeViewModel("A Level");

        actual.Title.Should().Be("Add A levels");
        actual.GroupTitle.Should().Be("A levels");
        actual.AllowMultipleAdd.Should().BeTrue();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
    }
    [Test]
    public void Then_Values_Set_For_AsLevel()
    {
        var actual = new QualificationDisplayTypeViewModel("As Level");

        actual.Title.Should().Be("Add AS levels");
        actual.GroupTitle.Should().Be("AS levels");
        actual.AllowMultipleAdd.Should().BeTrue();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
    }
    [Test]
    public void Then_Values_Set_For_TLevel()
    {
        var actual = new QualificationDisplayTypeViewModel("T Level");

        actual.Title.Should().Be("Add a T level");
        actual.GroupTitle.Should().Be("T levels");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
    }
    [Test]
    public void Then_Values_Set_For_Degree()
    {
        var actual = new QualificationDisplayTypeViewModel("Degree");

        actual.Title.Should().Be("Add a degree");
        actual.GroupTitle.Should().Be("Degree");
        actual.SubjectLabel.Should().Be("Degree");
        actual.AdditionalInformationLabel.Should().Be("University");
        actual.SubjectHintText.Should().Be("For example, BSc Mechanical Engineering");
        actual.GroupTitle.Should().Be("Degree");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeFalse();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeTrue();
    }
    
    [Test]
    public void Then_Values_Set_For_GCSE()
    {
        var actual = new QualificationDisplayTypeViewModel("GCse");

        actual.Title.Should().Be("Add GCSEs");
        actual.GroupTitle.Should().Be("GCSEs");
        actual.AllowMultipleAdd.Should().BeTrue();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
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