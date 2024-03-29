﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;
public class WhenGettingAddACourseRequest
{
    [Test, MoqAutoData]
    public void Then_View_Returned(
    Guid applicationId,
    [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
        .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
        .Returns("https://baseUrl");

        var controller = new TrainingCoursesController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };

        var actual = controller.GetAddATrainingCourse(applicationId) as ViewResult;
        var actualModel = actual?.Model as AddJobViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();
            actualModel?.ApplicationId.Should().Be(applicationId);
        }
    }
}
