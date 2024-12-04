using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SFA.DAS.FAA.Application.Commands.SavedSearches.PostSaveSearch;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenPostingSaveSearch
{
    private SearchApprenticeshipsController _sut;
    private Mock<IDataProtectorService> _dataProtectorService;
    private Mock<IMediator> _mediator;
    private static string RedirectUrl => "https://localhost/foo";
    private static Guid CandidateId => Guid.NewGuid();
    
    [SetUp]
    public void Setup()
    {
        _mediator = new Mock<IMediator>();
        _dataProtectorService = new Mock<IDataProtectorService>();
        var logger = new Mock<ILogger<SearchApprenticeshipsController>>();

        _sut = new SearchApprenticeshipsController(
            _mediator.Object,
            Mock.Of<IDateTimeService>(),
            Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(),
            Mock.Of<ICacheStorageService>(),
            Mock.Of<SearchModelValidator>(),
            Mock.Of<GetSearchResultsRequestValidator>(),
            _dataProtectorService.Object,
            logger.Object
        );
        
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(RedirectUrl);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.Request.Headers.Referer).Returns(new StringValues(RedirectUrl));

        _sut.Url = mockUrlHelper.Object;
        _sut.AddControllerContext().WithUser(CandidateId);
    }
    
    [Test, MoqAutoData]
    public async Task When_Valid_State_And_No_Redirect_Required_Then_The_Search_Parameters_Are_Saved_And_A_JsonResult_Is_Returned(
        Guid saveSearchId,
        string encodedString,
        GetSearchResultsRequest getSearchResultsRequest,
        SaveSearchRequest saveSearchRequest)
    {
        // arrange
        string? passedData = null;
        var json = JsonConvert.SerializeObject(getSearchResultsRequest);
        _dataProtectorService
            .Setup(x => x.DecodeData(It.IsAny<string>()))
            .Callback<string?>(x => passedData = x)
            .Returns(json);

        _dataProtectorService.Setup(x => x.EncodedData(saveSearchId.ToString()))
            .Returns(encodedString);
        
        SaveSearchCommand? passedCommand = null;
        _mediator
            .Setup(x => x.Send(It.IsAny<SaveSearchCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((cmd, _) => passedCommand = cmd as SaveSearchCommand);
        
        // act
        var result = await _sut.SaveSearch(saveSearchRequest, false) as JsonResult;
        
        // assert
        result?.Value.Should().Be((int)HttpStatusCode.OK);
        passedData.Should().Be(saveSearchRequest.Data);
        passedCommand.Should().BeEquivalentTo(getSearchResultsRequest, options => 
            options
                .WithMapping("LevelIds", "SelectedLevelIds")
                .WithMapping("RouteIds", "SelectedRouteIds")
                .WithMapping("Sort", "SortOrder")
                .WithMapping("UnSubscribeToken", encodedString)
                .Excluding(x => x.RoutePath)
                .Excluding(x => x.PageNumber)
                .Excluding(x => x.IncludeCompetitiveSalaryVacancies)
            );
    }
    
    [Test, MoqAutoData]
    public async Task When_Valid_State_And_Redirect_Required_Then_The_Search_Parameters_Are_Saved_And_A_RedirectResult_Is_Returned(
        Guid saveSearchId,
        string encodedString,
        GetSearchResultsRequest getSearchResultsRequest,
        SaveSearchRequest saveSearchRequest)
    {
        // arrange
        string? passedData = null;
        var json = JsonConvert.SerializeObject(getSearchResultsRequest);
        _dataProtectorService
            .Setup(x => x.DecodeData(It.IsAny<string>()))
            .Callback<string?>(x => passedData = x)
            .Returns(json);
        
        SaveSearchCommand? passedCommand = null;
        _mediator
            .Setup(x => x.Send(It.IsAny<SaveSearchCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((cmd, _) => passedCommand = cmd as SaveSearchCommand);

        _dataProtectorService.Setup(x => x.EncodedData(saveSearchId.ToString()))
            .Returns(encodedString);

        // act
        var result = await _sut.SaveSearch(saveSearchRequest) as RedirectResult;
        
        // assert
        result?.Url.Should().Be(RedirectUrl);
        passedData.Should().Be(saveSearchRequest.Data);
        passedCommand.Should().BeEquivalentTo(getSearchResultsRequest, options => 
            options
                .WithMapping("LevelIds", "SelectedLevelIds")
                .WithMapping("RouteIds", "SelectedRouteIds")
                .WithMapping("Sort", "SortOrder")
                .WithMapping("UnSubscribeToken", encodedString)
                .Excluding(x => x.RoutePath)
                .Excluding(x => x.PageNumber)
                .Excluding(x => x.IncludeCompetitiveSalaryVacancies)
        );
    }
    
    [Test, MoqAutoData]
    public async Task When_There_Is_No_Search_State_Then_A_Redirect_Result_Is_Returned_And_Nothing_Is_Saved(
        SaveSearchRequest saveSearchRequest)
    {
        // arrange
        saveSearchRequest.Data = null;
        
        // act
        var result = await _sut.SaveSearch(saveSearchRequest, false) as RedirectResult;
        
        // assert
        result?.Url.Should().Be(RedirectUrl);
        _mediator.Verify(x => x.Send(It.IsAny<SaveSearchCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task When_The_Search_State_Cannot_Be_Decoded_Then_A_Redirect_Result_Is_Returned_And_Nothing_Is_Saved(
        GetSearchResultsRequest getSearchResultsRequest,
        SaveSearchRequest saveSearchRequest)
    {
        // arrange
        _dataProtectorService
            .Setup(x => x.DecodeData(It.IsAny<string>()))
            .Returns((string?)null);
        
        // act
        var result = await _sut.SaveSearch(saveSearchRequest, false) as RedirectResult;
        
        // assert
        result?.Url.Should().Be(RedirectUrl);
        _mediator.Verify(x => x.Send(It.IsAny<SaveSearchCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task When_An_Exception_IS_Thrown_Saving_Then_A_Redirect_Result_Is_Returned(
        GetSearchResultsRequest getSearchResultsRequest,
        SaveSearchRequest saveSearchRequest)
    {
        // arrange
        var json = JsonConvert.SerializeObject(getSearchResultsRequest);
        _dataProtectorService
            .Setup(x => x.DecodeData(It.IsAny<string>()))
            .Returns(json);
        _mediator
            .Setup(x => x.Send(It.IsAny<SaveSearchCommand>(), It.IsAny<CancellationToken>()))
            .Throws<Exception>();
        
        // act
        var result = await _sut.SaveSearch(saveSearchRequest, false) as RedirectResult;
        
        // assert
        result?.Url.Should().Be(RedirectUrl);
    }
}