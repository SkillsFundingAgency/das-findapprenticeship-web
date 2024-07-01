using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;

public class WhenHandlingGetCandidatePostcodeQuery
{
    [Test, MoqAutoData]
    public async Task Ten_The_Api_Is_Called_And_Data_Returned(
        GetCandidatePostcodeQuery query,
        GetCandidatePostcodeApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetCandidatePostcodeQueryHandler handler)
    {
        apiClient.Setup(x =>
                x.Get<GetCandidatePostcodeApiResponse>(
                    It.Is<GetCandidatePostcodeApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Postcode.Should().Be(apiResponse.Postcode);
    }
}