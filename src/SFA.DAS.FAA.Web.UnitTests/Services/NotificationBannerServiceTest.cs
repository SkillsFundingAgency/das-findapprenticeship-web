using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Services
{
    [TestFixture]
    public class NotificationBannerServiceTest
    {
        [Test, MoqAutoData]
        public async Task ShowAccountCreatedBanner_When_Cache_Returns_True_Then_Return_True(
            string key, 
            bool showBanner,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            showBanner = true;
            cacheStorageService
                .Setup(x => x.Get<bool>(key))
                .ReturnsAsync(showBanner);

            //sut
            var result = await NotificationBannerService.ShowAccountBanner(cacheStorageService.Object, key);

            //assert
            result.Should().Be(showBanner);
            cacheStorageService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ShowAccountCreatedBanner_When_Cache_Returns_False_Then_Return_False(
            string key,
            bool showBanner,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            showBanner = false;
            cacheStorageService
                .Setup(x => x.Get<bool>(key))
                .ReturnsAsync(showBanner);

            //sut
            var result = await NotificationBannerService.ShowAccountBanner(cacheStorageService.Object, key);

            //assert
            result.Should().Be(showBanner);
            cacheStorageService.Verify(x => x.Remove(It.IsAny<string>()), Times.Never);
        }
    }
}