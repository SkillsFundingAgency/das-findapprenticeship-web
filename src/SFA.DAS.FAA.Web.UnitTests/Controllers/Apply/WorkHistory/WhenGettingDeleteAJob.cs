using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteJob;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    public class WhenGettingDeleteAJob
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Called_And_Delete_Jov_View_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        Guid jobId,
        GetDeleteJobQueryResult result,
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
            mediator.Setup(x => x.Send(It.IsAny<GetDeleteJobQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.GetDeleteJob(applicationId, jobId) as ViewResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual!.Model as DeleteJobViewModel;
        }
    }
}
