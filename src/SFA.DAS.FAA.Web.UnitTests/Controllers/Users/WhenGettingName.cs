using AutoFixture.NUnit3;
using CreateAccount.GetCandidateName;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

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
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, govIdentifier),
                        new(CustomClaims.CandidateId, candidateId.ToString())
                    }))
            }
        };
        mediator.Setup(x => x.Send(It.Is<GetCandidateNameQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.Name(journeyPath) as ViewResult;

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