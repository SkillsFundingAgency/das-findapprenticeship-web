using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;
public class WhenHandlingGetInterviewAdjustmentsQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetInterviewAdjustmentsQuery query,
        GetInterviewAdjustmentsApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetInterviewAdjustmentsQueryHandler handler)
    {
        var apiRequestUri = new GetInterviewAdjustmentsApiRequest(query.ApplicationId, query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetInterviewAdjustmentsApiResponse>(
                    It.Is<GetInterviewAdjustmentsApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.InterviewAdjustmentsDescription.Should().BeEquivalentTo(apiResponse.InterviewAdjustmentsDescription);
            result.Status.Should().Be(apiResponse.Status);
        }
    }
}
