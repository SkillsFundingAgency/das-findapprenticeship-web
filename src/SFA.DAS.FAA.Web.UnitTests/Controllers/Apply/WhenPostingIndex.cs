using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply;
public class WhenPostingIndex
{
    [Test, MoqAutoData]
    public async Task Then_RedirectToRoute_Returned(
        Guid applicationId,
        [Greedy] ApplyController controller)
    {
        var actual = controller.Index(applicationId) as RedirectToRouteResult;

        actual.RouteName.Should().Be(RouteNames.ApplyApprenticeship.ApplicationSubmitted);
    }
}
