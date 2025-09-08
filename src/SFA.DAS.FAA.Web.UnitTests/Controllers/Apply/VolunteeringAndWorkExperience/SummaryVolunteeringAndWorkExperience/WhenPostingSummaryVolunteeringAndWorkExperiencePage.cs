using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience.SummaryVolunteeringAndWorkExperience;

[TestFixture]
public class WhenPostingSummaryVolunteeringAndWorkExperiencePage
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid candidateId,
        VolunteeringAndWorkExperienceSummaryViewModel viewModel,
        UpdateVolunteeringAndWorkExperienceApplicationCommandResult result,
        Mock<IValidator<VolunteeringAndWorkExperienceSummaryViewModel>> validator,
        [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new VolunteeringAndWorkExperienceController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };
        
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        mediator.Setup(x => x.Send(It.Is<UpdateVolunteeringAndWorkExperienceApplicationCommand>(c =>
                c.ApplicationId.Equals(viewModel.ApplicationId)
                && c.CandidateId.Equals(candidateId)
                && c.VolunteeringAndWorkExperienceSectionStatus.Equals(viewModel.IsSectionCompleted)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<VolunteeringAndWorkExperienceSummaryViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Summary(validator.Object, viewModel) as RedirectToRouteResult;

        // assert
        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.Apply);
    }
}