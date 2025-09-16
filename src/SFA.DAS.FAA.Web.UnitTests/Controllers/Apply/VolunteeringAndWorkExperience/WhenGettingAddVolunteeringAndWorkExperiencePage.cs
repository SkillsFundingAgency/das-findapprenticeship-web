using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience;

public class WhenGettingAddVolunteeringAndWorkExperiencePage
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid applicationId,
        Guid candidateId,
        GetVolunteeringAndWorkExperiencesQueryResult result,
        [Frozen] Mock<IMediator> mediator)
    {
         // arrange
        result.VolunteeringAndWorkExperiences = new List<GetVolunteeringAndWorkExperiencesQueryResult.VolunteeringAndWorkExperience>();
        var controller = new VolunteeringAndWorkExperienceController(mediator.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(c =>
                c.ApplicationId.Equals(applicationId)
                && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // act
        var actual = await controller.Get(applicationId) as ViewResult;
        var actualModel = actual?.Model as VolunteeringAndWorkExperienceViewModel;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(applicationId);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Redirect_Route_Returned_When_Result_Is_Not_Null(
        Guid candidateId,
        Guid applicationId,
        GetVolunteeringAndWorkExperiencesQueryResult result,
        [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        var controller = new VolunteeringAndWorkExperienceController(mediator.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(c =>
                c.ApplicationId.Equals(applicationId)
                && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // act
        var actual = await controller.Get(applicationId) as RedirectToRouteResult;

        // assert
        actual!.RouteName.Should().NotBeNull();
        actual.RouteName.Should().Be(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary);
    }
}