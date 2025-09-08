using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.WorkHistory.UpdateJob;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory;

[TestFixture]
public class WhenPostingUpdateJobRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid candidateId,
        EditJobViewModel request,
        Mock<IValidator<EditJobViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WorkHistoryController controller)
    {
        // arrange
        controller.WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<UpdateJobCommand>(c=>
                c.JobId.Equals(request.JobId)
                && c.ApplicationId.Equals(request.ApplicationId)
                && c.CandidateId.Equals(candidateId)
                && c.EmployerName.Equals(request.EmployerName)
                && c.StartDate.Equals(request.StartDate)
                && c.EndDate.Equals(request.EndDate)
                && c.JobDescription.Equals(request.JobDescription)
                && c.JobTitle.Equals(request.JobTitle)
            ), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<EditJobViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.PostEditAJob(validator.Object, request) as RedirectToRouteResult;
            
        // assert
        actual.Should().NotBeNull();
    }
}