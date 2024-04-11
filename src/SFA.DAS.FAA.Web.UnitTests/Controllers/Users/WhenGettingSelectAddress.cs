using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingSelectAddress
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        string postcode,
        GetAddressesByPostcodeQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator, 
        [Greedy] UserController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAddressesByPostcodeQuery>(x => x.Postcode == postcode), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var result = await controller.SelectAddress(postcode) as ViewResult;
        var resultModel = result.Model as SelectAddressViewModel;

        resultModel.Addresses.Count.Should().Be(queryResult.Addresses.Count());
    }
}
