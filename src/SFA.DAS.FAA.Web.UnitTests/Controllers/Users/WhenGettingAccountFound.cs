using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidateStatus;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using System.Security.Claims;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    public class WhenGettingAccountFound
    {
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            string govIdentifier,
            string email,
            UpdateCandidateStatusCommandResult response,
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
                    }))

                }
            };

            mediator.Setup(x => x.Send(It.Is<UpdateCandidateStatusCommand>(x => x.GovIdentifier == govIdentifier && x.CandidateEmail == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await controller.AccountFound() as ViewResult;
            result.Should().NotBeNull();

            var actualModel = result!.Model as AccountFoundInformViewModel;

            actualModel.Should().NotBeNull();
            actualModel!.FirstName.Should().Be(response.FirstName);
        }
    }
}
