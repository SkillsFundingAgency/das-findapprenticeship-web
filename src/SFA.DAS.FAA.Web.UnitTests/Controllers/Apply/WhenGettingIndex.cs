using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply
{
    [TestFixture]
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
            GetIndexQueryResult result,
            GetIndexRequest request,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.ApplyController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetIndexQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Index(request) as ViewResult;

            Assert.That(actual, Is.Not.Null);

            var expected = IndexViewModel.Map(dateTimeService, request, result);

            var actualModel = actual.Model as IndexViewModel;
            actualModel.Should().BeEquivalentTo(expected, options => options
                .Excluding(x => x.ClosingDate));
        }
    }
}
