using System.Text;
using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
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
        public async Task Then_The_Data_Is_Cached(
            string key,
            TestObject objectToCache,
            [Frozen] Mock<IDistributedCache> mockCache,
            CacheStorageService sut)
        {
            // Act
            await sut.Set(key, objectToCache);

            // Assert
            mockCache.Verify(x =>
                    x.SetAsync(
                        key,
                        It.Is<byte[]>(c =>
                            Encoding.UTF8.GetString(c)
                                .Equals(JsonSerializer.Serialize(objectToCache, new JsonSerializerOptions()))),
                        It.Is<DistributedCacheEntryOptions>(c
                            => c.AbsoluteExpirationRelativeToNow.Value.Minutes == TimeSpan.FromMinutes(60).Minutes
                            && c.SlidingExpiration.Value.Minutes == TimeSpan.FromMinutes(30).Minutes),
                        It.IsAny<CancellationToken>()),
                Times.Once);
            
        }

        public class TestObject
        {
            public string TestValue { get; set; }
        }
    }
}