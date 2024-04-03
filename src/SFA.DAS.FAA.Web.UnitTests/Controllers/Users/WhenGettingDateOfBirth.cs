using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidateDateOfBirth;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingDateOfBirth
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(string govIdentifier, [Frozen] Mock<IMediator> mediator, [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier)
                    }))

            }
        };

        mediator.Setup(x => x.Send(It.IsAny<GetCandidateDateOfBirthQuery>(), CancellationToken.None))
            .ReturnsAsync(new GetCandidateDateOfBirthQueryResult { DateOfBirth = null});

        var result = await controller.DateOfBirth() as ViewResult;

        result.Should().NotBeNull();
        mediator.Verify(x => x.Send(It.IsAny<GetCandidateDateOfBirthQuery>(), CancellationToken.None), Times.Once());
    }
}
