using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingEnterAddressManually
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        string? postcode,
        string backLink,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        var result = controller.EnterAddressManually(backLink, postcode) as ViewResult;
        var resultModel = result.Model as EnterAddressManuallyViewModel;

        resultModel.BackLink.Should().BeEquivalentTo(backLink);
    }
}
