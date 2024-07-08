using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingMapSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Map_Data_Returned(
        string searchTerm,
        string location,
        Guid candidateId,
        string govIdentifier,
        bool disabilityConfident,
        List<string>? routeIds,
        List<string>? levelIds,
        GetSearchResultsResult result,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IUrlHelper> mockUrlHelper)
    {
        int? distance = 10;
        dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow);
        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c =>
                c.SearchTerm!.Equals(searchTerm)
                && c.Location!.Equals(location)
                && c.SelectedRouteIds!.Equals(routeIds)
                && c.PageSize!.Value == 300
                && c.DisabilityConfident!.Equals(disabilityConfident)
                && c.Distance.Equals(distance)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        
        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object, faaConfig.Object, cacheStorageService.Object, Mock.Of<SearchModelValidator>(), Mock.Of<GetSearchResultsRequestValidator>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier)
                    }))
                }
            }
        };
        
        var actual = await controller.MapSearchResults(new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            LevelIds = levelIds,
            DisabilityConfident = disabilityConfident
        }) as JsonResult;

        actual.Should().NotBeNull();
        var actualValue = actual!.Value as MapSearchResultsViewModel;
        actualValue.Should().NotBeNull();
        actualValue.ApprenticeshipMapData.Should().BeEquivalentTo(result.Vacancies
            .Select(c => new ApprenticeshipMapData().MapToViewModel(dateTimeService.Object, c)).ToList());
        actualValue.SearchedLocation.Should().BeEquivalentTo(result.Location);
    }
}