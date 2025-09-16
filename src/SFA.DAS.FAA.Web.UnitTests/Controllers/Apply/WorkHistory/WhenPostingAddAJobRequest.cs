using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory;

[TestFixture]
public class WhenPostingAddAJobRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid candidateId,
        AddJobViewModel request,
        AddJobCommandResponse result,
        Mock<IValidator<AddJobViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WorkHistoryController controller)
    {
        // arrange
        controller.WithContext(x => x
            .WithUser(candidateId)
            .WithClaim(CustomClaims.CandidateId, candidateId.ToString()));
        
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
            
        validator
            .Setup(x => x.ValidateAsync(It.IsAny<AddJobViewModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.PostAddAJob(validator.Object, request) as RedirectToRouteResult;
            
        // assert
        actual.Should().NotBeNull();
    }
}