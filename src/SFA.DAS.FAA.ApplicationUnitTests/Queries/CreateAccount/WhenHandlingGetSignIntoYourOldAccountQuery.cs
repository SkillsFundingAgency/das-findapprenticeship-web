using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetSignIntoYourOldAccount;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount
{
    [TestFixture]
    public class WhenHandlingGetSignIntoYourOldAccountQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetSignIntoYourOldAccountQuery query,
            GetSignIntoYourOldAccountApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            var apiRequestUri = new GetSignIntoYourOldAccountApiRequest(query.CandidateId, query.Email, query.Password);

            apiClientMock.Setup(client =>
                    client.Get<GetSignIntoYourOldAccountApiResponse>(
                        It.Is<GetSignIntoYourOldAccountApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsValid.Should().Be(apiResponse.IsValid);
        }
    }
}
