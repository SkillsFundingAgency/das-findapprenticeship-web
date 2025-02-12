using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    [TestFixture]
    public class WhenGettingAccountDeletionWithDrawApplication
    {
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            Guid candidateId,
            string email,
            GetAccountDeletionApplicationsToWithdrawQueryResult response,
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

            mediator.Setup(x => x.Send(It.Is<GetAccountDeletionApplicationsToWithdrawQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await controller.AccountDeletionWithDrawApplications() as ViewResult;
            result.Should().NotBeNull();

            var actualModel = result!.Model as AccountDeletionWithDrawApplicationsViewModel;

            actualModel.Should().NotBeNull();
            actualModel!.SubmittedApplications.Count.Should().Be(response.SubmittedApplications.Count);
            actualModel.SubmittedApplications.Should().BeEquivalentTo(response.SubmittedApplications, options => options
                .Excluding(x => x.SubmittedDate)
                .Excluding(x => x.VacancyReference)
                .Excluding(x => x.WorkLocation)
                .Excluding(x => x.ClosingDate)
                .Excluding(x => x.Status)
                .Excluding(x => x.CreatedDate)
                .Excluding(x => x.Addresses)
                .Excluding(x => x.EmployerLocationOption)
                .Excluding(x => x.EmploymentLocationInformation)
            );
        }

        [Test, MoqAutoData]
        public async Task Then_Redirect_Is_Returned_As_Expected(
            Guid candidateId,
            string email,
            GetAccountDeletionApplicationsToWithdrawQueryResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            response.SubmittedApplications = [];
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

            mediator.Setup(x => x.Send(It.Is<GetAccountDeletionApplicationsToWithdrawQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await controller.AccountDeletionWithDrawApplications() as RedirectToRouteResult;
            result.Should().NotBeNull();
            result!.RouteName.Should().Be(RouteNames.AccountDelete);
        }
    }
}
