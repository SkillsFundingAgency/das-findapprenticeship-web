using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.UpdateApplication;
public class WhenHandlingUpdateVolunteeringAndWorkExperienceApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
    UpdateVolunteeringAndWorkExperienceApplicationCommand command,
    Domain.Apply.UpdateApplication.Application apiResponse,
    [Frozen] Mock<IApiClient> apiClientMock,
    UpdateVolunteeringAndWorkExperienceApplicationCommandHandler handler)
    {
        var expectedPostRequest = new UpdateVolunteeringAndWorkExperienceApplicationApiRequest(command.ApplicationId, command.CandidateId, new UpdateVolunteeringAndWorkHistoryApplicationModel
        {
            VolunteeringAndWorkExperienceSectionStatus = command.VolunteeringAndWorkExperienceSectionStatus
        });
        apiClientMock.Setup(client => client.Post<Domain.Apply.UpdateApplication.Application>(expectedPostRequest)).ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Application.Should().NotBeNull();
            result.Application.Should().BeEquivalentTo(apiResponse);
        }
    }

    [Test, MoqAutoData]
    public async Task And_ApiResponse_Is_Empty_Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateVolunteeringAndWorkExperienceApplicationCommand command,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateVolunteeringAndWorkExperienceApplicationCommandHandler handler)
    {
        var expectedPostRequest = new UpdateVolunteeringAndWorkExperienceApplicationApiRequest(command.ApplicationId, command.CandidateId, new UpdateVolunteeringAndWorkHistoryApplicationModel
        {
            VolunteeringAndWorkExperienceSectionStatus = command.VolunteeringAndWorkExperienceSectionStatus
        });
        apiClientMock.Setup(client => client.Post<Domain.Apply.UpdateApplication.Application>(expectedPostRequest)).ReturnsAsync(() => null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
    }
}
