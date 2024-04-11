using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.User;
public class WhenHandlingGetCandidateAccountDetailsQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidateAccountDetailsQuery query,
        GetCandidateCheckAnswersApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidateAccountDetailsQueryHandler handler)
    {
        apiClientMock.Setup(client =>
                client.Get<GetCandidateCheckAnswersApiResponse>(
                    It.Is<GetCandidateCheckAnswersApiRequest>(c =>
                        c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        var expectedResult = new
        {
            apiResponse.AddressLine1,
            apiResponse.AddressLine2,
            apiResponse.Town,
            apiResponse.County,
            apiResponse.Postcode,
            apiResponse.DateOfBirth,
            apiResponse.FirstName,
            apiResponse.LastName,
            CandidatePreferences = apiResponse.CandidatePreferences.Select(x => new
            {
                x.PreferenceId,
                Meaning = x.PreferenceMeaning,
                Hint = x.PreferenceHint,
                EmailPreference = x.ContactMethodsAndStatus?.Where(x => x.ContactMethod == Constants.Constants.CandidatePreferences.ContactMethodEmail).FirstOrDefault()?.Status ?? false,
                TextPreference = x.ContactMethodsAndStatus?.Where(x => x.ContactMethod == Constants.Constants.CandidatePreferences.ContactMethodText).FirstOrDefault()?.Status ?? false
            }).ToList()
        };

        result.Should().BeEquivalentTo(expectedResult);
    }
}
