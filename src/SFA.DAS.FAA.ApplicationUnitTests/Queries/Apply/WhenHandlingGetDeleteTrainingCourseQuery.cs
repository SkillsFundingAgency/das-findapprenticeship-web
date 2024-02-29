using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;
public class WhenHandlingGetDeleteTrainingCourseQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
    GetDeleteTrainingCourseQuery query,
    GetDeleteTrainingCourseApiResponse apiResponse,
    [Frozen] Mock<IApiClient> apiClientMock,
    GetDeleteTrainingCourseQueryHandler handler)
    {
        var apiRequestUri = new GetDeleteTrainingCourseApiRequest(query.ApplicationId, query.CandidateId, query.TrainingCourseId);

        apiClientMock.Setup(client =>
                client.Get<GetDeleteTrainingCourseApiResponse>(
                    It.Is<GetDeleteTrainingCourseApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(apiResponse);
        }
    }
}
