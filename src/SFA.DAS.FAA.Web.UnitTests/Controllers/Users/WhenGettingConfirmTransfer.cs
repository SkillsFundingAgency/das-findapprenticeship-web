using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Application.Queries.Applications.GetTransferUserData;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    public class WhenGettingConfirmTransfer
    {
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            string email,
            Guid candidateId,
            GetTransferUserDataQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new(CustomClaims.CandidateId, candidateId.ToString()),
                        new(ClaimTypes.Email, email)
                    }))
                }
            };

            mediator.Setup(x => x.Send(It.Is<GetTransferUserDataQuery>(x => x.CandidateId == candidateId && x.EmailAddress == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var result = await controller.ConfirmDataTransfer() as ViewResult;
            var resultModel = result!.Model as ConfirmTransferViewModel;

            resultModel.Should().NotBeNull();

            using var scope = new AssertionScope();
            resultModel!.EmailAddress.Should().Be(queryResult.CandidateEmailAddress);
            resultModel.Name.Should().Be(queryResult.CandidateFirstName);
            resultModel.SavedApplicationsCount.Should().Be(queryResult.SavedApplications);
            resultModel.StartedApplicationsCount.Should().Be(queryResult.StartedApplications);
            resultModel.SubmittedApplicationsCount.Should().Be(queryResult.SubmittedApplications);
        }
    }
}
