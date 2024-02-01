using MediatR;
using SFA.DAS.FAA.Domain.Apply.AddJob;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob
{
    public class AddJobCommand : IRequest<AddJobCommandResponse>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class AddJobCommandResponse
    {
        public Guid Id { get; set; }

    }

    public class AddJobCommandHandler : IRequestHandler<AddJobCommand, AddJobCommandResponse>
    {
        private readonly IApiClient _apiClient;

        public AddJobCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AddJobCommandResponse> Handle(AddJobCommand request, CancellationToken cancellationToken)
        {
            var data = new PostJobApiRequest.PostJobApiRequestData
            {
                CandidateId = request.CandidateId,
                EmployerName = request.EmployerName,
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                JobTitle = request.JobTitle,
                JobDescription = request.JobDescription
            };

            var apiRequest = new PostJobApiRequest(request.ApplicationId, data);

            var apiResponse = await _apiClient.PostWithResponseCode<PostJobApiResponse>(apiRequest);

            return new AddJobCommandResponse
            {
                Id = apiResponse.Id
            };
        }
    }
}
