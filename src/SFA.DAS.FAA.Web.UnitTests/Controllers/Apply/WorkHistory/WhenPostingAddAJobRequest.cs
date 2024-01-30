using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    [TestFixture]
    public class WhenPostingAddAJobRequest
    {
        [Test, MoqAutoData]
        public async Task Then_RedirectRoute_Returned(
            Guid candidateId,
            AddJobViewModel request,
            AddJobCommandResponse result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.WorkHistoryController controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            mediator.Setup(x => x.Send(It.Is<AddJobCommand>(c=>
                    c.ApplicationId.Equals(request.ApplicationId)
                    && c.CandidateId.Equals(candidateId)
                    && c.EmployerName.Equals(request.EmployerName)
                    && c.StartDate.Equals(request.StartDate)
                    && c.EndDate.Equals(request.EndDate)
                    && c.JobDescription.Equals(request.JobDescription)
                    && c.JobTitle.Equals(request.JobTitle)

                    ), It.IsAny<CancellationToken>()))
                
                .ReturnsAsync(result);

            var actual = await controller.PostAddAJob(request) as RedirectToRouteResult;
            actual.Should().NotBeNull();
        }
    }
}
