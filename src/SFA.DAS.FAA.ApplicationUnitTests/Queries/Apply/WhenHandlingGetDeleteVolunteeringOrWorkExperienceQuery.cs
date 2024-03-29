﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;
public class WhenHandlingGetDeleteVolunteeringOrWorkExperienceQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
          GetVolunteeringOrWorkExperienceItemQuery query,
          GetVolunteeringOrWorkExperienceItemApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
          GetVolunteeringOrWorkExperienceItemQueryHandler handler)
    {
        var apiRequestUri = new GetVolunteeringOrWorkExperienceItemApiRequest(query.ApplicationId, query.Id, query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetVolunteeringOrWorkExperienceItemApiResponse>(
                    It.Is<GetVolunteeringOrWorkExperienceItemApiRequest>(c =>
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