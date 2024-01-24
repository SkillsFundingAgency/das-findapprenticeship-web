using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Web.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    [TestFixture]
    public class WhenGettingRequest
    {
        [Test, MoqAutoData]
        public async Task Then_View_Returned(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IValidator<AddWorkHistoryRequest>> validator)
        {
            //arrange
            var request = new AddWorkHistoryRequest
            {
                VacancyReference = "01234567890",
                ApplicationId = Guid.NewGuid(),
                AddJob = "Yes",
            };

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

            var controller = new WorkHistoryController(mediator.Object, validator.Object)
            {
                Url = mockUrlHelper.Object
            };

            validator.Setup(x => x.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            var actual = await controller.Get(request) as ViewResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();

            var actualModel = actual?.Model as AddWorkHistoryRequest;
            actualModel?.VacancyReference.Should().Be(request.VacancyReference);
            actualModel?.ApplicationId.Should().Be(request.ApplicationId);
            actualModel?.AddJob.Should().Be(request.AddJob);
        }

        [Test, MoqAutoData]
        public async Task Then_Request_With_ValidationError_Is_Called_And_View_Returned(
            AddWorkHistoryRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IValidator<AddWorkHistoryRequest>> validator)
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

            var controller = new WorkHistoryController(mediator.Object, validator.Object)
            {
                Url = mockUrlHelper.Object
            };

            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("SomeProperty", "SomeError"));

            validator.Setup(x => x.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);

            var actual = await controller.Get(request) as ViewResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            var actualModel = actual?.Model as AddWorkHistoryRequest; 
            actualModel?.ErrorDictionary.Should().NotBeNull();
        }
    }
}
