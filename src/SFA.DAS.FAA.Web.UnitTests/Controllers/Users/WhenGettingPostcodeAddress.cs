using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingPostcodeAddress
{
    [Test, MoqAutoData]
    public void Then_View_Is_Returned([Frozen] Mock<IMediator> mediator, [Greedy] UserController controller)
    {
        var result = controller.PostcodeAddress() as ViewResult;

        result.Should().NotBeNull();
    }
}
