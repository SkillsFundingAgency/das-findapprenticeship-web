using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WhatInterestsYou;

[TestFixture]
public class WhenGettingRequest765
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid applicationId,
        Guid candidateId,
        GetWhatInterestsYouQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        //arrange
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetWhatInterestsYouQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new WhatInterestsYouController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        var actual = await controller.Get(applicationId) as ViewResult;

        using var scope = new AssertionScope();
        actual.Should().NotBeNull();
        actual?.Model.Should().NotBeNull();

        var actualModel = actual?.Model as WhatInterestsYouViewModel;
        actualModel?.ApplicationId.Should().Be(applicationId);
        actualModel?.EmployerName.Should().Be(queryResult.EmployerName);
        actualModel?.StandardName.Should().Be(queryResult.StandardName);
    }
}