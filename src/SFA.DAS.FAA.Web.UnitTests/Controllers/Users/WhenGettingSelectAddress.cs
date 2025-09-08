using CreateAccount.GetAddressesByPostcode;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenGettingSelectAddress
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.Settings)]
    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        Guid candidateId,
        string postcode,
        GetAddressesByPostcodeQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator, 
        [Greedy] UserController controller)
    {
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        mediator.Setup(x => x.Send(It.Is<GetAddressesByPostcodeQuery>(x => x.Postcode == postcode && x.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var result = await controller.SelectAddress(postcode, journeyPath) as ViewResult;
        var resultModel = result.Model as SelectAddressViewModel;

        resultModel.Addresses.Count.Should().Be(queryResult.Addresses.Count());
        resultModel.JourneyPath.Should().Be(journeyPath);
    }

    [Test]
    [MoqInlineAutoData(null)]
    [MoqInlineAutoData("")]
    public async Task Then_If_The_Postcode_Is_Null_Or_Empty_Redirect_To_Enter_Postcode(
        string postcode,
        Guid candidateId,
        GetAddressesByPostcodeQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        var result = await controller.SelectAddress(postcode) as RedirectToRouteResult;

        result!.RouteName.Should().Be(RouteNames.PostcodeAddress);
        mediator.Verify(x => x.Send(It.IsAny<GetAddressesByPostcodeQuery>(), CancellationToken.None), Times.Never);
    }
}