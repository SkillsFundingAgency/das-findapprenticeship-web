using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.WhatInterestsYou;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WhatInterestsYou;

[TestFixture]
public class WhenPostingRequest
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
        Guid candidateId,
        Guid applicationId,
        WhatInterestsYouViewModel request,
        Mock<IValidator<WhatInterestsYouViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WhatInterestsYouController controller)
    {
        // arrange
        controller.WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.IsAny<UpdateWhatInterestsYouCommand>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<WhatInterestsYouViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, applicationId, request) as RedirectToRouteResult;

        // assert
        using var scope = new AssertionScope();
        actual.Should().NotBeNull();
        actual?.RouteName.Should().Be(RouteNames.Apply);
        actual?.RouteValues.Should().NotBeEmpty();
    }
        
    [Test, MoqAutoData]
    public async Task And_Autosaving_JsonResult_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        WhatInterestsYouViewModel request,
        Mock<IValidator<WhatInterestsYouViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WhatInterestsYouController controller)
    {
        //arrange
        controller.WithContext(x => x.WithUser(candidateId));

        mediator
            .Setup(x => x.Send(It.IsAny<UpdateWhatInterestsYouCommand>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);
            
        request.AutoSave = true;
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<WhatInterestsYouViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, applicationId, request) as JsonResult;

        // assert
        actual.Should().NotBeNull();
    }
}