using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Applications;

public class WhenGettingWithdrawView
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_View_Returned(
        Guid candidateId,
        Guid applicationId,
        GetWithdrawApplicationQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetWithdrawApplicationQuery>(c =>
                c.CandidateId.Equals(candidateId) && c.ApplicationId.Equals(applicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
        }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var actual = await controller.Withdraw(applicationId) as ViewResult;

        actual.Should().NotBeNull();
        var actualModel = actual.Model as WithdrawApplicationViewModel;
        actualModel.ApplicationId.Should().Be(result.ApplicationId);
        actualModel.AdvertTitle.Should().Be(result.AdvertTitle);
        actualModel.EmployerName.Should().Be(result.EmployerName);
    }
}