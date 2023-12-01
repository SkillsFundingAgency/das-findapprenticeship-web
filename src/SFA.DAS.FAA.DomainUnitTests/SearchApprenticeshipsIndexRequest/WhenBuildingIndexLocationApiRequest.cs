using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchApprenticeshipsIndexRequest;

public class WhenBuildingIndexLocationApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string searchTerm)
    {
        var actual = new IndexLocationApiRequest(searchTerm);

        actual.GetUrl.Should().Be($"searchapprenticeships/indexlocation?locationSearchTerm={searchTerm}");
    }
}