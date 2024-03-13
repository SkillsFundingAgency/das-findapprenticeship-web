using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

public class WhenGettingQualificationTypes
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned_And_Qualification_Reference_Types_Added_To_ViewModel(
        Guid applicationId,
        Guid candidateId,
        GetQualificationTypesQueryResponse queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetQualificationTypesQuery>(), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.AddQualificationSelectType(applicationId) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        actual!.ViewName.Should().Be("~/Views/apply/Qualifications/AddQualificationSelectType.cshtml");
        var actualModel = actual.Model as AddQualificationSelectTypeViewModel;
        actualModel!.ApplicationId.Should().Be(applicationId);
        actualModel!.Qualifications.Should().BeEquivalentTo(queryResult.QualificationTypes);
    }
}