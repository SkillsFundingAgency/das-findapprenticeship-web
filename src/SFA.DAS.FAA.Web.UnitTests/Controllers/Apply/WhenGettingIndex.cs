using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
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
            Guid candidateId,
            GetIndexQueryResult result,
            GetIndexRequest request,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplyController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetIndexQuery>(c=>
                    c.CandidateId.Equals(candidateId)
                    && c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(CustomClaims.CandidateId, candidateId.ToString()),
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.Index(request) as ViewResult;

            Assert.That(actual, Is.Not.Null);
            var expected = IndexViewModel.Map(dateTimeService.Object, request, result);
            var actualModel = actual.Model as IndexViewModel;
            actualModel.Should().BeEquivalentTo(expected, options => options
                .Excluding(x => x.ClosingDate)
                .Excluding(x => x.ApplicationId));
            actualModel?.ApplicationId.Should().Be(request.ApplicationId);
        }
    }
}
