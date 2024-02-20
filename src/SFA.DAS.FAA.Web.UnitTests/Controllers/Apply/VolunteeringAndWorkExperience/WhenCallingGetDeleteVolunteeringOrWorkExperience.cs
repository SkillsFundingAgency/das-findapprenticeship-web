using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience;
public class WhenCallingGetDeleteVolunteeringOrWorkExperience
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Delete_VolunteeringOrWorkExperience_View_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        Guid id,
        GetVolunteeringOrWorkExperienceItemQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.Apply.VolunteeringAndWorkExperienceController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        mediator.Setup(x => x.Send(It.IsAny<GetVolunteeringOrWorkExperienceItemQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.GetDeleteVolunteeringOrWorkExperience(applicationId, id) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as DeleteVolunteeringOrWorkExperienceViewModel;
    }
}
