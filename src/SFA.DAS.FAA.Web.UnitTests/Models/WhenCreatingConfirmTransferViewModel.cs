using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetTransferUserData;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingConfirmTransferViewModel
{
    [Test, AutoData]
    public void Then_If_The_Text_Is_Formatted_Correctly_For_Greater_Than_One(GetTransferUserDataQueryResult source)
    {
        source.SavedApplications = 10;
        source.StartedApplications = 10;
        source.SubmittedApplications = 10;
        
        var actual = (ConfirmTransferViewModel)source;

        actual.Name.Should().Be(source.CandidateFirstName);
        actual.SavedApplicationsText.Should().Be(" saved vacancies");
        actual.SubmittedApplicationsText.Should().Be(" submitted applications");
        actual.StartedApplicationsText.Should().Be(" started applications");
        actual.SubmittedApplicationsCount.Should().Be(source.SubmittedApplications);
        actual.SavedApplicationsCount.Should().Be(source.SavedApplications);
        actual.StartedApplicationsCount.Should().Be(source.StartedApplications);
    }
    [Test, AutoData]
    public void Then_If_The_Text_Is_Formatted_Correctly_For_One(GetTransferUserDataQueryResult source)
    {
        source.SavedApplications = 1;
        source.StartedApplications = 1;
        source.SubmittedApplications = 1;
        
        var actual = (ConfirmTransferViewModel)source;

        actual.Name.Should().Be(source.CandidateFirstName);
        actual.SavedApplicationsText.Should().Be(" saved vacancy");
        actual.SubmittedApplicationsText.Should().Be(" submitted application");
        actual.StartedApplicationsText.Should().Be(" started application");
        actual.SubmittedApplicationsCount.Should().Be(source.SubmittedApplications);
        actual.SavedApplicationsCount.Should().Be(source.SavedApplications);
        actual.StartedApplicationsCount.Should().Be(source.StartedApplications);
    }
    [Test, AutoData]
    public void Then_If_The_Text_Is_Formatted_Correctly_For_None(GetTransferUserDataQueryResult source)
    {
        source.SavedApplications = 0;
        source.StartedApplications = 0;
        source.SubmittedApplications = 0;
        
        var actual = (ConfirmTransferViewModel)source;

        actual.Name.Should().Be(source.CandidateFirstName);
        actual.SavedApplicationsText.Should().BeEmpty();
        actual.SubmittedApplicationsText.Should().BeEmpty();
        actual.StartedApplicationsText.Should().BeEmpty();
        actual.SubmittedApplicationsCount.Should().Be(source.SubmittedApplications);
        actual.SavedApplicationsCount.Should().Be(source.SavedApplications);
        actual.StartedApplicationsCount.Should().Be(source.StartedApplications);
    }
}