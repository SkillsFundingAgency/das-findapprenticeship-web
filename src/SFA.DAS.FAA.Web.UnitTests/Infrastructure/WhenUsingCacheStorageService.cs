using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Infrastructure
{
    [TestFixture]
    public class WhenUsingCacheStorageService
    {
        [Test, MoqAutoData]
        public void Then_The_Data_Is_Cached(
            string key,
            string objectToCache)
        {
            //Arrange
            var mockCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            mockCache
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Callback((object k) => _ = (string)k)
                .Returns(mockCacheEntry.Object);

            mockCacheEntry
                .SetupSet(mce => mce.Value = It.IsAny<object>())
                .Callback<object>(v => _ = v);

            mockCacheEntry
                .SetupSet(mce => mce.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan?>())
                .Callback<TimeSpan?>(dto => _ = dto);

            // Act
            var sut = new CacheStorageService(mockCache.Object);
            
            var actual = sut.Set(key, objectToCache);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(objectToCache);
        }
    }
}