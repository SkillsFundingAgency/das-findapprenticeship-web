using System.Security.Claims;
using CreateAccount.GetCandidateName;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenGettingName
{
    [Test]
    [MoqInlineAutoData(null, RouteNames.CreateAccount, "What is your name? – Find an apprenticeship – GOV.UK", "Create an account", "What is your name?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.CreateAccount, "What is your name? – Find an apprenticeship – GOV.UK", "Create an account", "What is your name?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails, "What is your name? – Find an apprenticeship – GOV.UK", "Create an account", "What is your name?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings, "Change your name – Find an apprenticeship – GOV.UK", "", "Change your name", "Save")]
    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        string pageBackLink,
        string pageTitle,
        string pageCaption,
        string pageHeading,
        string pageCtaButtonLabel,
        Guid candidateId,
        string govIdentifier,
        GetCandidateNameQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // arrange
        controller
            .AddControllerContext()
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier);
        mediator.Setup(x => x.Send(It.Is<GetCandidateNameQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        // act
        var result = await controller.Name(journeyPath) as ViewResult;

        // assert
        result.Should().NotBeNull();
        var actualModel = result!.Model as NameViewModel;
        
        actualModel.Should().NotBeNull();
        actualModel!.PageTitle.Should().Be(pageTitle);
        actualModel.PageCaption.Should().Be(pageCaption);
        actualModel.PageHeading.Should().Be(pageHeading);
        actualModel.PageCtaButtonLabel.Should().Be(pageCtaButtonLabel);
        actualModel.JourneyPath.Should().Be(journeyPath);
        actualModel.BackLink.Should().Be(pageBackLink);
    }
}