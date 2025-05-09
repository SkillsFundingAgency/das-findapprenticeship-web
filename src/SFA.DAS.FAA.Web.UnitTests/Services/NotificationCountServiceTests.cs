using MediatR;
using SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.UnitTests.Services
{
    [TestFixture]
    public class NotificationCountServiceTests
    {
        [Test, MoqAutoData]
        public async Task GetUnreadApplicationCount_ShouldReturnCorrectCount_WhenSomeNotificationsAreUnread(
            Guid candidateId,
            GetApplicationsCountQuery query,
            GetApplicationsCountQueryResult queryResult,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
            [Greedy] NotificationCountService service)
        {
            // Arrange
            var status = ApplicationStatus.Successful;
            var applicationIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            var readIds = new List<Guid> { applicationIds[0] };
            query = new GetApplicationsCountQuery(candidateId, status);

            mediatorMock
                .Setup(m => m.Send(It.Is<GetApplicationsCountQuery>(c => c.CandidateId == candidateId && c.Status == status), CancellationToken.None))
                .ReturnsAsync(queryResult);

            cacheStorageServiceMock
                .Setup(c => c.Get<List<Guid>>(It.IsAny<string>()))
                .ReturnsAsync(readIds);

            // Act
            var result = await service.GetUnreadApplicationCount(candidateId, status);

            // Assert
            result.Should().Be(3);
        }

        [Test, MoqAutoData]
        public async Task GetUnreadApplicationCount_ShouldReturnZero_WhenAllNotificationsAreRead(
            Guid candidateId,
            GetApplicationsCountQuery query,
            GetApplicationsCountQueryResult queryResult,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
            [Greedy] NotificationCountService service)
        {
            // Arrange
            var status = ApplicationStatus.Submitted;
            var applicationIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var readIds = new List<Guid>(applicationIds);
            queryResult = new GetApplicationsCountQueryResult
            {
                Stats = new ApplicationStatusCount(applicationIds, 2, status)
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<GetApplicationsCountQuery>(c => c.CandidateId == candidateId && c.Status == status), CancellationToken.None))
                .ReturnsAsync(queryResult);

            cacheStorageServiceMock
                .Setup(c => c.Get<List<Guid>>(It.IsAny<string>()))
                .ReturnsAsync(readIds);

            // Act
            var result = await service.GetUnreadApplicationCount(candidateId, status);

            // Assert
            result.Should().Be(0);
        }

        [Test, MoqAutoData]
        public async Task GetUnreadApplicationCount_ShouldReturnAll_WhenNoNotificationsAreRead(
            Guid candidateId,
            GetApplicationsCountQuery query,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
            [Greedy] NotificationCountService service)
        {
            // Arrange
            var status = ApplicationStatus.Submitted;
            var applicationIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            mediatorMock
                .Setup(m => m.Send(It.Is<GetApplicationsCountQuery>(c => c.CandidateId == candidateId && c.Status == status), CancellationToken.None))
                .ReturnsAsync(new GetApplicationsCountQueryResult
                {
                    Stats = new ApplicationStatusCount(applicationIds, 3, status)
                });

            cacheStorageServiceMock
                .Setup(c => c.Get<List<Guid>>(It.IsAny<string>()))
                .ReturnsAsync([]);

            // Act
            var result = await service.GetUnreadApplicationCount(candidateId, status);

            // Assert
            result.Should().Be(2);
        }

        [Test, MoqAutoData]
        public async Task MarkAllNotificationsAsRead_ShouldNotSetCache_WhenNoApplicationsExist(
            Guid candidateId,
            GetApplicationsCountQuery query,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
            [Greedy] NotificationCountService service)
        {
            // Arrange
            var status = ApplicationStatus.Submitted;

            mediatorMock
                .Setup(m => m.Send(It.Is<GetApplicationsCountQuery>(c => c.CandidateId == candidateId && c.Status == status), CancellationToken.None))
                .ReturnsAsync(new GetApplicationsCountQueryResult
                {
                    Stats = new ApplicationStatusCount([], 0, status)
                });

            // Act
            await service.MarkAllNotificationsAsRead(candidateId, status);

            // Assert
            cacheStorageServiceMock.Verify(
                c => c.Set(It.IsAny<string>(), It.IsAny<List<Guid>>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task MarkAllNotificationsAsRead_ShouldSetCache_WhenApplicationsExist(Guid candidateId,
            GetApplicationsCountQuery query,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
            [Greedy] NotificationCountService service)
        {
            // Arrange
            var status = ApplicationStatus.Submitted;
            var applicationIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            mediatorMock
                .Setup(m => m.Send(It.Is<GetApplicationsCountQuery>(c => c.CandidateId == candidateId && c.Status == status), CancellationToken.None))
                .ReturnsAsync(new GetApplicationsCountQueryResult
                {
                    Stats = new ApplicationStatusCount(applicationIds, applicationIds.Count, status)
                });

            // Act
            await service.MarkAllNotificationsAsRead(candidateId, status);

            // Assert
            cacheStorageServiceMock.Verify(
                c => c.Set(
                    It.Is<string>(key => key.Contains(candidateId.ToString()) && key.Contains(status.ToString())),
                    It.Is<List<Guid>>(ids => ids.SequenceEqual(applicationIds)),
                    It.Is<int>(sliding => sliding == 30),
                    It.Is<int>(absolute => absolute == 60)),
                Times.Once);
        }
    }
}
