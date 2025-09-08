using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenGettingPostcodeAddress
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails)]
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
        controller
            .AddControllerContext()
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier);

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
        controller
            .AddControllerContext()
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier);

        mediator.Setup(x => x.Send(It.Is<GetCandidatePostcodeQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.PostcodeAddress(postcode, journeyPath) as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result.Model as PostcodeAddressViewModel;
        actualModel.Postcode.Should().Be(postcode);
        actualModel.JourneyPath.Should().Be(journeyPath);
    }
}