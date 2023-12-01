using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;

public class WhenGettingIndexLocation
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_Api_Called_And_Data_Returned(
        GetIndexLocationQuery query,
        IndexLocationApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetIndexLocationQueryHandler handler)
    {
        apiClient.Setup(x =>
                x.Get<IndexLocationApiResponse>(
                    It.Is<IndexLocationApiRequest>(c =>
                        c.GetUrl.Contains($"?locationSearchTerm={HttpUtility.UrlEncode(query.LocationSearchTerm)}"))))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Location.Should().BeEquivalentTo(apiResponse.Location);
    }
}