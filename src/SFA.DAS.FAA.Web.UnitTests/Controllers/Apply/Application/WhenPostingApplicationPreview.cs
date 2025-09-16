using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Commands.SubmitApplication;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.ApplicationStatus;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Application;

[TestFixture]
public class WhenPostingApplicationPreview
{
    [Test, MoqAutoData]
    public async Task Then_Redirect_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        UpdateApplicationStatusCommandResult updateApplicationStatusCommandResult,
        Mock<IValidator<ApplicationSummaryViewModel>> validator,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator)
    {
        var request = new ApplicationSummaryViewModel
        {
            ApplicationId = applicationId,
            IsConsentProvided = true,
        };

        var controller = new ApplyController(mediator.Object, cacheStorageService.Object, dateTimeService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));
            
        validator
            .Setup(x => x.ValidateAsync(It.Is<ApplicationSummaryViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var actual = await controller.Preview(validator.Object, applicationId, request) as RedirectToRouteResult;
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().NotBeNull();
            actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.ApplicationSubmitted);
        }
        mediator.Verify(x => x.Send(It.Is<SubmitApplicationCommand>(c =>
            c.ApplicationId.Equals(applicationId) &&
            c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()), Times.Once);
    }
}