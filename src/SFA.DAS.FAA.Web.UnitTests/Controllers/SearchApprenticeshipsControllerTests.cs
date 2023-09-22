using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Vacancies.Queries;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers;

public class SearchApprenticeshipsControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_View_Returned(
        GetSearchApprenticeshipsIndexResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetSearchApprenticeshipsIndexQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        
        var actual = await controller.Index() as ViewResult;

        Assert.IsNotNull(actual);
        actual!.Model.Should().BeEquivalentTo((SearchApprenticeshipsViewModel) result);
    }
}