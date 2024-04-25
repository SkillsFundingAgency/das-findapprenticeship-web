using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

public class WhenHandlingGetAddQualificationQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetModifyQualificationApiResponse apiResponse,
        GetModifyQualificationQuery query,
        [Frozen] Mock<IApiClient> apiClient,
        GetModifyQualificationQueryHandler handler)
    {
        apiClient.Setup(x =>
                x.Get<GetModifyQualificationApiResponse>(
                    It.Is<GetModifyQualificationApiRequest>(c => 
                        c.GetUrl.Contains(query.QualificationReferenceId.ToString()) 
                        && c.GetUrl.Contains(query.ApplicationId.ToString())
                        && c.GetUrl.Contains(query.CandidateId.ToString())
                        && c.GetUrl.Contains(query.QualificationId.ToString()!)
                        )))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.QualificationType.Should().BeEquivalentTo(apiResponse.QualificationType);
        actual.Qualifications.Should().BeEquivalentTo(apiResponse.Qualifications);
    }
}