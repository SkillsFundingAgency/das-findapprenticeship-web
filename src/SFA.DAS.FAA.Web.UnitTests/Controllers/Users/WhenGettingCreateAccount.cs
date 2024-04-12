using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingCreateAccount
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned([Frozen] Mock<IMediator> mediator, string returnUrl, [Greedy] UserController controller)
    {
        var result = controller.CreateAccount(returnUrl) as ViewResult;

        result.Should().NotBeNull();
    }
}
