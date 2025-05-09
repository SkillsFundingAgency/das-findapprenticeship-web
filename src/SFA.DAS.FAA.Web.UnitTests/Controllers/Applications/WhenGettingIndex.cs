using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Security.Claims;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Applications
{
    [TestFixture]
    public class WhenGettingIndex
    {
        [Test]
        [MoqInlineAutoData(ApplicationsTab.Started)]
        public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
            ApplicationsTab tab,
            int successCount,
            int unSuccessCount,
            Guid candidateId,
            GetIndexQueryResult result,
            string bannerMessage,
            string applicationSubmittedBannerMessage,
            string govIdentifier,
            bool showEqualityQuestionsBanner,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<INotificationCountService> notificationCountService,
            [Greedy] ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetIndexQuery>(c =>
                    c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-VacancyWithdrawn"))
                .ReturnsAsync(bannerMessage);
            cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-ApplicationSubmitted"))
                .ReturnsAsync(applicationSubmittedBannerMessage);

            notificationCountService.Setup(x => x.GetUnreadApplicationCount(candidateId, ApplicationStatus.Successful))
                .ReturnsAsync(successCount);
            notificationCountService.Setup(x => x.GetUnreadApplicationCount(candidateId, ApplicationStatus.Unsuccessful))
                .ReturnsAsync(unSuccessCount);

            var user = new ClaimsPrincipal(new ClaimsIdentity([
                new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, govIdentifier)
            ]));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.Index(showEqualityQuestionsBanner: showEqualityQuestionsBanner) as ViewResult;

            Assert.That(actual, Is.Not.Null);
            var expected = IndexViewModel.Map(tab, result, dateTimeService.Object);
            var actualModel = actual.Model as IndexViewModel;
            actualModel.Should().BeEquivalentTo(expected, options  =>options
                .Excluding(c=>c.WithdrawnBannerMessage)
                .Excluding(c => c.ApplicationSubmittedBannerMessage)
                .Excluding(c => c.ShowEqualityQuestionsBannerMessage)
                .Excluding(c => c.NewUnsuccessfulApplicationsCount)
                .Excluding(c => c.NewSuccessfulApplicationsCount)
            );
            actualModel!.WithdrawnBannerMessage.Should().Be(bannerMessage);
            actualModel!.ApplicationSubmittedBannerMessage.Should().Be(applicationSubmittedBannerMessage);
            actualModel!.ShowEqualityQuestionsBannerMessage.Should().Be(showEqualityQuestionsBanner);
            actualModel.NewUnsuccessfulApplicationsCount.Should().Be(unSuccessCount);
            actualModel.NewSuccessfulApplicationsCount.Should().Be(successCount);

            cacheStorageService.Verify(x=>x.Remove($"{govIdentifier}-VacancyWithdrawn"), Times.Once);
            cacheStorageService.Verify(x => x.Remove($"{govIdentifier}-ApplicationSubmitted"), Times.Once);

            notificationCountService.Verify(x => x.MarkAllNotificationsAsRead(candidateId, ApplicationStatus.Successful), Times.Never);
            notificationCountService.Verify(x => x.MarkAllNotificationsAsRead(candidateId, ApplicationStatus.Unsuccessful), Times.Never);
        }
    }
}
