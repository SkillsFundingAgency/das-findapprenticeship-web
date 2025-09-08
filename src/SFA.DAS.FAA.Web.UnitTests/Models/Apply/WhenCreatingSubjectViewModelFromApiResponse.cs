using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenCreatingSubjectViewModelFromApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetQualificationsApiResponse.Qualification source)
    {
        var actual = (SubjectViewModel)source;

        actual.Id.Should().Be(source.Id);
        actual.Grade.Should().Be(source.Grade);
        actual.Name.Should().Be(source.Subject);
        actual.Level.Should().Be(source.AdditionalInformation);
        actual.IsPredicted.Should().Be(source.IsPredicted);
        actual.AdditionalInformation.Should().Be(source.AdditionalInformation);
    }
}