using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
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
            var memoryCache = Mock.Of<IMemoryCache>();
            var cacheEntry = Mock.Of<ICacheEntry>();

            var mockMemoryCache = Mock.Get(memoryCache);
            mockMemoryCache
                .Setup(m => m.CreateEntry(key))
            .Returns(cacheEntry);

            //Act
            var actual = memoryCache.Set<string>(key, objectToCache);

            //Assert
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(objectToCache);
        }
    }
}