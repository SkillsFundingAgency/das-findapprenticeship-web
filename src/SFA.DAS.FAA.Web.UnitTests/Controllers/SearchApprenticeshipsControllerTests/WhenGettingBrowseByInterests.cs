using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenGettingBrowseByInterests
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Browse_By_Interests_View_Returned(
    GetBrowseByInterestsResult result,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetBrowseByInterestsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.BrowseByInterests() as ViewResult;

        Assert.IsNotNull(actual);
        var actualModel = actual!.Model as BrowseByInterestViewModel;
        actualModel.Should().BeEquivalentTo((BrowseByInterestViewModel)result);
    }
}
