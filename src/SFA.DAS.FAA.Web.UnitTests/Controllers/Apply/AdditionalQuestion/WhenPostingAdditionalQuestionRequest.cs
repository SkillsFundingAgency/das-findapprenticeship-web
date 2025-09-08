using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.AdditionalQuestion;

[TestFixture]
public class WhenPostingAdditionalQuestionRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid applicationId,
        Guid candidateId,
        Guid additionalQuestionId,
        int additionalQuestion,
        Mock<IValidator<AddAdditionalQuestionViewModel>> validator,
        AddAdditionalQuestionViewModel model,
        AddAdditionalQuestionCommandHandlerResult result,
        [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        mediator.Setup(x => x.Send(It.Is<AddAdditionalQuestionCommand>(c =>
                    c.ApplicationId == applicationId
                    && c.CandidateId == candidateId
                    && c.Id == additionalQuestionId
                    && c.Answer == model.AdditionalQuestionAnswer
                    && c.UpdatedAdditionalQuestion == additionalQuestion
                    && c.AdditionalQuestionSectionStatus == SectionStatus.Completed)
                , It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var controller = new AdditionalQuestionController(mediator.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));
        
        validator
            .Setup(x => x.ValidateAsync(It.IsAny<AddAdditionalQuestionViewModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, applicationId, additionalQuestion, additionalQuestionId, model) as RedirectToRouteResult;
        
        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().Be(RouteNames.Apply);
        }
    }
}