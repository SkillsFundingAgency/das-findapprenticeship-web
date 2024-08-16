using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using CreateAccount.GetCandidateAccountDetails;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingConfirmYourAccountDetails
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, "Create an account", "Create your account")]
    [MoqInlineAutoData(UserJourneyPath.AccountFound, "", "Continue")]
    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        string pageCaption,
        string pageCtaButtonLabel,
        string govIdentifier,
        Guid candidateId,
        string phoneNumber,
        GetCandidateAccountDetailsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.MobilePhone, phoneNumber)
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidateAccountDetailsQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.CheckAnswers(journeyPath) as ViewResult;
        var resultModel = result.Model as ConfirmAccountDetailsViewModel;

        resultModel.Postcode.Should().BeEquivalentTo(queryResult.Postcode);
        resultModel!.JourneyPath.Should().Be(journeyPath);
        resultModel.PageCaption.Should().Be(pageCaption);
        resultModel.PageCtaButtonLabel.Should().Be(pageCtaButtonLabel);
    }
}
