using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

public class WhenHandlingGetQualificationTypesQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Data_Returned(
        GetQualificationTypesApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetQualificationTypesQueryHandler handler)
    {
        apiClient.Setup(x => x.Get<GetQualificationTypesApiResponse>(It.IsAny<GetQualificationTypesApiRequest>()))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(new GetQualificationTypesQuery(), CancellationToken.None);

        actual.QualificationTypes.Should().BeEquivalentTo(apiResponse.QualificationTypes.OrderBy(c=>c.Order), options=> options.WithStrictOrderingFor(c=>c));
    }
}