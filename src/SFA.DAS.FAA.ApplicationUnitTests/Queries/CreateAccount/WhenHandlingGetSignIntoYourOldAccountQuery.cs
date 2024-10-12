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
            PostSignIntoYourOldAccountApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            var apiRequestUri = new PostSignIntoYourOldAccountApiRequest(new PostSignIntoYourOldAccountApiRequestData
            {
                CandidateId = query.CandidateId,
                Email = query.Email,
                Password = query.Password
            });

            apiClientMock.Setup(client =>
                    client.PostWithResponseCode<PostSignIntoYourOldAccountApiResponse>(
                        It.Is<PostSignIntoYourOldAccountApiRequest>(c =>
                            c.PostUrl == apiRequestUri.PostUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsValid.Should().Be(apiResponse.IsValid);
        }

        [Test, MoqAutoData]
        public async Task Then_Result_Is_Null_IsValid_False_Returned(
            GetSignIntoYourOldAccountQuery query,
            PostSignIntoYourOldAccountApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            var apiRequestUri = new PostSignIntoYourOldAccountApiRequest(new PostSignIntoYourOldAccountApiRequestData
            {
                CandidateId = query.CandidateId,
                Email = query.Email,
                Password = query.Password
            });

            apiClientMock.Setup(client =>
                    client.PostWithResponseCode<PostSignIntoYourOldAccountApiResponse>(
                        It.Is<PostSignIntoYourOldAccountApiRequest>(c =>
                            c.PostUrl == apiRequestUri.PostUrl)))
                .ReturnsAsync(() => null);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
        }
    }
}
