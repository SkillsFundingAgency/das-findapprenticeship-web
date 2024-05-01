using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenCreatingCourseDataListItem
{
    [Test, AutoData]
    public void Then_Fields_Are_Mapped(CourseApiResponse source)
    {
        source.IsStandard = false;
        
        var actual = (CourseDataListItem)source;

        actual.Id.Should().Be(source.Id);
        actual.Title.Should().Be(source.Title);
    }
    
}