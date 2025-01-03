using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteTrainingCourse;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

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
        controller.AddControllerContext().WithUser(query.CandidateId);

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
    
    [Test, MoqAutoData]
    public async Task Then_Browser_Is_Redirected_When_Training_Course_Not_Found(
        GetDeleteTrainingCourseQuery query,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        controller.AddControllerContext().WithUser(query.CandidateId);

        mediator
            .Setup(x => x.Send(It.IsAny<GetDeleteTrainingCourseQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetDeleteTrainingCourseQueryResult)null);

        var actual = await controller.Delete(query.ApplicationId, query.TrainingCourseId) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.TrainingCourses);
    }
}
