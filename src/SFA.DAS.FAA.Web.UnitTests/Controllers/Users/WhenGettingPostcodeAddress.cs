using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingPostcodeAddress
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.AccountFound)]
    [MoqInlineAutoData(UserJourneyPath.Settings)]
    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        string email,
        Guid candidateId,
        string govIdentifier,
        string? postcode,
        GetCandidatePostcodeQueryResult queryResult,
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
                        new Claim(ClaimTypes.Email, email),
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidatePostcodeQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.PostcodeAddress((string)null!, journeyPath) as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result.Model as PostcodeAddressViewModel;
        actualModel.Postcode.Should().Be(queryResult.Postcode);
        actualModel.JourneyPath.Should().Be(journeyPath);
    }
    
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.AccountFound)]
    [MoqInlineAutoData(UserJourneyPath.Settings)]
    public async Task Then_View_Is_Returned_And_Postcode_Shown_If_Passed(
        UserJourneyPath journeyPath,
        string email,
        Guid candidateId,
        string govIdentifier,
        string? postcode,
        GetCandidatePostcodeQueryResult queryResult,
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
                    new Claim(ClaimTypes.Email, email),
                    new Claim(CustomClaims.CandidateId, candidateId.ToString())
                }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidatePostcodeQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.PostcodeAddress(postcode, journeyPath) as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result.Model as PostcodeAddressViewModel;
        actualModel.Postcode.Should().Be(postcode);
        actualModel.JourneyPath.Should().Be(journeyPath);
    }
}
