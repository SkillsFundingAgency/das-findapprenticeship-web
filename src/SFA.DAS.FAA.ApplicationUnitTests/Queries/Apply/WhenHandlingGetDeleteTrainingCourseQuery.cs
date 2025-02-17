﻿using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;

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
    
    [Test, MoqAutoData]
    public async Task Then_Null_Result_Is_Handled(
        GetDeleteTrainingCourseQuery query,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetDeleteTrainingCourseQueryHandler handler)
    {
        var apiRequestUri = new GetDeleteTrainingCourseApiRequest(query.ApplicationId, query.CandidateId, query.TrainingCourseId);

        apiClientMock
            .Setup(client => client.Get<GetDeleteTrainingCourseApiResponse>(It.Is<GetDeleteTrainingCourseApiRequest>(c => c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync((GetDeleteTrainingCourseApiResponse?)null);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}
