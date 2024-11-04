using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Service;

public class WhenSigningIn
{

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Decoded_And_Redirected(
        string controllerName,
        string actionName,
        string urlParam,
        Guid uniqueId,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Greedy]ServiceController controller)
    {
        var queryString = string.Empty;
        var decryptedValue = $"{controllerName}|{actionName}|{uniqueId}|{queryString}";
        dataProtectorService.Setup(x => x.DecodeData(urlParam)).Returns(decryptedValue);

        var actual = await controller.SignIn(urlParam) as RedirectToActionResult;

        actual.ActionName.Should().Be(actionName);
        actual.ControllerName.Should().Be(controllerName);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_QueryString_Is_Included_In_The_RouteValues(
        string controllerName,
        string actionName,
        string urlParam,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Greedy]ServiceController controller)
    {
        var decryptedValue = $"{controllerName}|{actionName}||values=1&values=2&foo=yes";
        dataProtectorService.Setup(x => x.DecodeData(urlParam)).Returns(decryptedValue);

        var actual = await controller.SignIn(urlParam) as RedirectToActionResult;

        actual.RouteValues?.Count.Should().Be(2);
        actual.RouteValues?.Should().ContainEquivalentOf(new KeyValuePair<string, object>("values", new StringValues([ "1", "2" ])));
        actual.RouteValues?.Should().ContainEquivalentOf(new KeyValuePair<string, object>("foo", new StringValues([ "yes" ])));
    }
    
    public async Task Then_The_VacancyRef_Is_Included_In_The_RouteValues(
        string controllerName,
        string actionName,
        string urlParam,
        Guid uniqueId,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Greedy]ServiceController controller)
    {
        var decryptedValue = $"{controllerName}|{actionName}|{uniqueId}|";
        dataProtectorService.Setup(x => x.DecodeData(urlParam)).Returns(decryptedValue);

        var actual = await controller.SignIn(urlParam) as RedirectToActionResult;

        actual.RouteValues?.Count.Should().Be(1);
        actual.RouteValues?.Should().ContainEquivalentOf(new KeyValuePair<string, string>("vacancyReference", uniqueId.ToString()));
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_It_Cant_Be_Decoded_Redirect_To_Index(
        string urlParam,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Greedy]ServiceController controller)
    {
        dataProtectorService.Setup(x => x.DecodeData(urlParam)).Returns((string)null);

        var actual = await controller.SignIn(urlParam) as RedirectToRouteResult;

        actual.RouteName.Should().Be(RouteNames.ServiceStartDefault);
    }
}