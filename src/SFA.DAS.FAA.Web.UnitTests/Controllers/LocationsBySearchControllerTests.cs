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

namespace SFA.DAS.FAA.Web.UnitTests.Controllers;
public class LocationsBySearchControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Sent_And_Data_Retrieved_And_Json_Returned(
    string searchTerm,
    GetLocationsBySearchQueryResult response,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] LocationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetLocationsBySearchQuery>(l => l.SearchTerm.Equals(searchTerm)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var actual = await controller.Locations(searchTerm);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.As<JsonResult>().Should().NotBeNull();
            actual.As<LocationsBySearchViewModel>().Locations.Should().NotBeNull();
        }
    }
}
