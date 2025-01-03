using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Applications
{
    [TestFixture]
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
            Guid candidateId,
            GetIndexQueryResult result,
            string bannerMessage,
            string applicationSubmittedBannerMessage,
            string govIdentifier,
            bool showEqualityQuestionsBanner,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Greedy] ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetIndexQuery>(c =>
                    c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-VacancyWithdrawn"))
                .ReturnsAsync(bannerMessage);
            cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-ApplicationSubmitted"))
                .ReturnsAsync(applicationSubmittedBannerMessage);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(CustomClaims.CandidateId, candidateId.ToString()),
                new(ClaimTypes.NameIdentifier, govIdentifier)
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.Index(showEqualityQuestionsBanner: showEqualityQuestionsBanner) as ViewResult;

            Assert.That(actual, Is.Not.Null);
            var expected = IndexViewModel.Map(ApplicationsTab.Started, result, dateTimeService.Object);
            var actualModel = actual.Model as IndexViewModel;
            actualModel.Should().BeEquivalentTo(expected, options  =>options
                .Excluding(c=>c.WithdrawnBannerMessage)
                .Excluding(c => c.ApplicationSubmittedBannerMessage)
            );
            actualModel!.WithdrawnBannerMessage.Should().Be(bannerMessage);
            actualModel!.ApplicationSubmittedBannerMessage.Should().Be(applicationSubmittedBannerMessage);
            actualModel!.ShowEqualityQuestionsBannerMessage.Should().Be(showEqualityQuestionsBanner);
            cacheStorageService.Verify(x=>x.Remove($"{govIdentifier}-VacancyWithdrawn"), Times.Once);
            cacheStorageService.Verify(x => x.Remove($"{govIdentifier}-ApplicationSubmitted"), Times.Once);
        }
    }
}
