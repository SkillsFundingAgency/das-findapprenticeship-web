using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Apply.UpdateEmploymentLocations;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Commands.EmploymentLocations.Update
{
    public class UpdateEmploymentLocationsCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateEmploymentLocationsCommand>
    {
        public async Task Handle(UpdateEmploymentLocationsCommand command, CancellationToken cancellationToken)
        {
            // Fetch employment locations from the API
            var employmentLocationsApiResponse = await apiClient.Get<GetEmploymentLocationsApiResponse>(
                new GetEmploymentLocationsApiRequest(command.ApplicationId, command.CandidateId));

            // Update the selection status of addresses
            var updatedAddresses = command.SelectedAddressIds.Count > 0
                ? employmentLocationsApiResponse.EmploymentLocation.Addresses
                    .Select(address =>
                    {
                        address.IsSelected = command.SelectedAddressIds.Contains(address.Id);
                        return address;
                    })
                    .ToList()
                : employmentLocationsApiResponse.EmploymentLocation.Addresses;

            // Prepare the request payload for updating employment locations
            var postUpdateApplicationRequest = new PostEmploymentLocationsApiRequest(
                command.ApplicationId,
                new PostEmploymentLocationsApiRequest.PostEmploymentLocationsApiRequestData
                {
                    CandidateId = command.CandidateId,
                    EmployerLocation = new LocationDto
                    {
                        Addresses = updatedAddresses,
                        EmployerLocationOption = employmentLocationsApiResponse.EmploymentLocation.EmployerLocationOption,
                        EmploymentLocationInformation = employmentLocationsApiResponse.EmploymentLocation.EmploymentLocationInformation,
                        Id = employmentLocationsApiResponse.EmploymentLocation.Id
                    },
                    EmploymentLocationSectionStatus = command.EmploymentLocationSectionStatus
                });

            // Send the update request to the API
            await apiClient.Post(postUpdateApplicationRequest);
        }
    }
}