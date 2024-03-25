using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenBuildingQualificationDisplayTypeViewModel
{
    [Test]
    public void Then_Values_Set_For_Apprenticeship()
    {
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("Apprenticeship", id);

        actual.Id.Should().Be(id);
        actual.Title.Should().Be("Add an apprenticeship");
        actual.GroupTitle.Should().Be("Apprenticeships");
        actual.SubjectLabel.Should().Be("Training course");
        actual.SubjectHintText.Should().Be("For example, Network engineer (Level 4)");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeFalse();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
        actual.HasDataLookup.Should().BeTrue();
        actual.AddOrder.Should().Be(6);
        actual.ListOrder.Should().Be(2);
    }
    [Test]
    public void Then_Values_Set_For_Btec()
    {
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("BTEC", id);

        actual.Id.Should().Be(id);
        actual.Title.Should().Be("Add a BTEC");
        actual.GroupTitle.Should().Be("BTEC");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeTrue();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
        actual.AddOrder.Should().Be(2);
        actual.ListOrder.Should().Be(6);
    }
    [Test]
    public void Then_Values_Set_For_ALevel()
    {
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("A Level", id);

        actual.Id.Should().Be(id);
        actual.Title.Should().Be("Add A levels");
        actual.GroupTitle.Should().Be("A levels");
        actual.AllowMultipleAdd.Should().BeTrue();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
        actual.AddOrder.Should().Be(5);
        actual.ListOrder.Should().Be(3);
    }
    [Test]
    public void Then_Values_Set_For_AsLevel()
    {
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("As Level", id);

        actual.Id.Should().Be(id);
        actual.Title.Should().Be("Add AS levels");
        actual.GroupTitle.Should().Be("AS levels");
        actual.AllowMultipleAdd.Should().BeTrue();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
        actual.AddOrder.Should().Be(4);
        actual.ListOrder.Should().Be(4);
    }
    [Test]
    public void Then_Values_Set_For_TLevel()
    {
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("T Level", id);

        actual.Id.Should().Be(id);
        actual.Title.Should().Be("Add a T level");
        actual.GroupTitle.Should().Be("T levels");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeTrue();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeFalse();
        actual.AddOrder.Should().Be(3);
        actual.ListOrder.Should().Be(5);
    }
    [Test]
    public void Then_Values_Set_For_Degree()
    {
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("Degree", id);

        actual.Id.Should().Be(id);
        actual.Title.Should().Be("Add a degree");
        actual.GroupTitle.Should().Be("Degree");
        actual.SubjectLabel.Should().Be("Degree");
        actual.AdditionalInformationLabel.Should().Be("University");
        actual.SubjectHintText.Should().Be("For example, BSc Mechanical Engineering");
        actual.SelectHintText.Should().Be("(undergraduate, postgraduate or foundation)");
        actual.GroupTitle.Should().Be("Degree");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeFalse();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeTrue();
    }
    
    [Test]
    public void Then_Values_Set_For_GCSE()
    {
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("GCse", id);

        actual.Id.Should().Be(id);
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
        var id = Guid.NewGuid();
        
        var actual = new QualificationDisplayTypeViewModel("MAgic", id);

        actual.Id.Should().Be(id);
        actual.Title.Should().Be("Add other qualifications");
        actual.GroupTitle.Should().Be("Other qualifications");
        actual.AllowMultipleAdd.Should().BeFalse();
        actual.CanShowPredicted.Should().BeFalse();
        actual.CanShowLevel.Should().BeFalse();
        actual.ShouldDisplayAdditionalInformationField.Should().BeTrue();
        actual.SelectHintText.Should().Be("(including international qualifications)");
    }
}