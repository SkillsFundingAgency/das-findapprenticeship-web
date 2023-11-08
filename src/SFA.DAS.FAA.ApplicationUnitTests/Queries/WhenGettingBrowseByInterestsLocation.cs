using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FAA.Domain.BrowseByInterestsLocation;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;

public class WhenGettingBrowseByInterestsLocation
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_Api_Called_And_Data_Returned(
        GetBrowseByInterestsLocationQuery query,
        BrowseByInterestsLocationApiResponse apiResponse,
        [Frozen]Mock<IApiClient> apiClient,
        GetBrowseByInterestsLocationQueryHandler handler)
    {
        apiClient.Setup(x =>
                x.Get<BrowseByInterestsLocationApiResponse>(
                    It.Is<GetBrowseByInterestsLocationApiRequest>(c =>
                        c.GetUrl.Contains($"?locationSearchTerm={HttpUtility.UrlEncode(query.LocationSearchTerm)}"))))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Location.Should().BeEquivalentTo(apiResponse.Location);
    }
}