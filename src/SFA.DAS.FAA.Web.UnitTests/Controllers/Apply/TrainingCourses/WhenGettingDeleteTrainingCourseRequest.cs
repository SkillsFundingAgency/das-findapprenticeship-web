using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;
public class WhenGettingDeleteTrainingCourseRequest
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        GetDeleteTrainingCourseQuery query,
        GetDeleteTrainingCourseQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        result.ApplicationId = query.ApplicationId;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, query.CandidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetDeleteTrainingCourseQuery>(c =>
                c.ApplicationId == query.ApplicationId
                && c.CandidateId == query.CandidateId
                && c.TrainingCourseId == query.TrainingCourseId)
                , It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

        var actual = await controller.Delete(query.ApplicationId, query.TrainingCourseId) as ViewResult;
        var actualModel = actual?.Model as DeleteTrainingCourseViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();
            actualModel?.ApplicationId.Should().Be(query.ApplicationId);
        }
    }
}
