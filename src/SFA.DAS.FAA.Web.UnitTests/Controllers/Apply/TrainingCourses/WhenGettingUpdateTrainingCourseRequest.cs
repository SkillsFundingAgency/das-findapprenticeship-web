using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;

public class WhenGettingUpdateTrainingCourseRequest
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        GetTrainingCourseQuery query,
        GetTrainingCourseQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        result.ApplicationId = query.ApplicationId;
        controller.WithContext(x => x.WithUser(query.CandidateId));
        mediator.Setup(x => x.Send(It.Is<GetTrainingCourseQuery>(c =>
                c.ApplicationId == query.ApplicationId
                && c.CandidateId == query.CandidateId
                && c.TrainingCourseId == query.TrainingCourseId)
                , It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

        var actual = await controller.Edit(query.ApplicationId, query.TrainingCourseId) as ViewResult;
        var actualModel = actual?.Model as EditTrainingCourseViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();
            actualModel?.ApplicationId.Should().Be(query.ApplicationId);
        }
    }
}