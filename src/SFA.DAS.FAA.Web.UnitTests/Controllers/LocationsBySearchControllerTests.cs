using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FAA.Domain.LocationsBySearch.GetLocationsBySearchApiResponse;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers;
public class LocationsBySearchControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Sent_And_Data_Retrieved_And_Json_Returned(
    string searchTerm,
    List<LocationItem> locationItems,
    [Greedy] GetLocationsBySearchQueryResult response,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] LocationsController controller)
    {
        response.LocationItems = locationItems;
        mediator.Setup(x => x.Send(It.Is<GetLocationsBySearchQuery>(l => l.SearchTerm.Equals(searchTerm)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var actual = await controller.LocationsBySearch(searchTerm);
        var actualJsonResult = actual as JsonResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actualJsonResult!.Value.Should().BeOfType<LocationViewModel>();
            actualJsonResult.Should().NotBeNull();
            ((LocationViewModel)actualJsonResult!.Value!).Locations.Should().NotBeNull();
        }
    }
}
