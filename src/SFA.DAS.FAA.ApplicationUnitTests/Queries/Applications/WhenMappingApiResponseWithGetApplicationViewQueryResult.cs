using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Apply.GetApplicationView;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications
{
    [TestFixture]
    public class WhenMappingApiResponseWithGetApplicationViewQueryResult
    {
        [Test, MoqAutoData]
        public void Map_Returns_Expected_Result(
            GetApplicationViewApiResponse source)
        {
            var result = (GetApplicationViewQueryResult)source;

            using var scope = new AssertionScope();
            result.Should().BeEquivalentTo(source);
        }
    }
}
