using SFA.DAS.FAA.Application.Commands.EmploymentLocations.Update;
using SFA.DAS.FAA.Domain.Apply.EqualityQuestions;
using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Apply.UpdateEmploymentLocations;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using static SFA.DAS.FAA.Domain.Apply.UpdateEmploymentLocations.PostEmploymentLocationsApiRequest;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.EmploymentLocations
{
    [TestFixture]
    public class WhenHandlingUpdateEmploymentLocationsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            UpdateEmploymentLocationsCommand command,
            GetEmploymentLocationsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] UpdateEmploymentLocationsCommandHandler handler)
        {
            // Arrange
            apiClient.Setup(client =>
                    client.Get<GetEmploymentLocationsApiResponse>(
                        new GetEmploymentLocationsApiRequest(command.ApplicationId, command.CandidateId)))
                .ReturnsAsync(apiResponse);

            var updatedAddresses = command.SelectedAddressIds.Count > 0
                ? apiResponse.EmploymentLocation.Addresses
                    .Select(address =>
                    {
                        address.IsSelected = command.SelectedAddressIds.Contains(address.Id);
                        return address;
                    })
                    .ToList()
                : apiResponse.EmploymentLocation.Addresses;

            var postUpdateApplicationRequest = new PostEmploymentLocationsApiRequest(
                command.ApplicationId,
                new PostEmploymentLocationsApiRequestData
                {
                    CandidateId = command.CandidateId,
                    EmployerLocation = new LocationDto
                    {
                        Addresses = updatedAddresses,
                        EmployerLocationOption = apiResponse.EmploymentLocation.EmployerLocationOption,
                        EmploymentLocationInformation = apiResponse.EmploymentLocation.EmploymentLocationInformation,
                        Id = apiResponse.EmploymentLocation.Id
                    },
                    EmploymentLocationSectionStatus = command.EmploymentLocationSectionStatus
                });

            apiClient.Setup(client => client.Post<PostEqualityQuestionsApiResponse>(postUpdateApplicationRequest));

            await handler.Handle(command, It.IsAny<CancellationToken>());

            apiClient.Verify(x => x.Post(It.IsAny<PostEmploymentLocationsApiRequest>()), Times.Once);
        }
    }
}
