using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply;
public class WhenGettingApplicationSubmittedConfirmation
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
        Guid candidateId,
        Guid applicationId,
        GetApplicationSubmittedQueryResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplyController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetApplicationSubmittedQuery>(c =>
                c.CandidateId.Equals(candidateId)
                && c.ApplicationId.Equals(applicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
      
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(CustomClaims.CandidateId, candidateId.ToString()),
            }))}
        };

        var actual = await controller.ApplicationSubmittedConfirmation(applicationId) as ViewResult;

        actual?.Should().NotBeNull();
        var actualModel = actual!.Model as ApplicationSubmittedConfirmationViewModel;
        actualModel?.Should().NotBeNull();
        actualModel!.VacancyInfo.Should().NotBeNull();
        actualModel!.VacancyInfo.Should().BeEquivalentTo(result);
    }
}
