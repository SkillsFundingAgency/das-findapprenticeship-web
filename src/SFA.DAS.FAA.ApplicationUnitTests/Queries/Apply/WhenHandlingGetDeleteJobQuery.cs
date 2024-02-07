using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteJob;
using SFA.DAS.FAA.Application.Queries.Apply.GetJob;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingGetDeleteJobQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
          GetDeleteJobQuery query,
          GetDeleteJobApiResponse apiResponse,
          [Frozen] Mock<IApiClient> apiClientMock,
          GetDeleteJobQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetDeleteJobApiRequest(query.ApplicationId, query.CandidateId, query.JobId);

            apiClientMock.Setup(client =>
                    client.Get<GetDeleteJobApiResponse>(
                        It.Is<GetDeleteJobApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(apiResponse);
        }
    }
}
