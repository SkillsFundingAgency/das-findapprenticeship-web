using MediatR;
using SFA.DAS.FAA.Domain.Apply.AddJob;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.WorkHistory.UpdateJob
{
    public class UpdateJobCommand : IRequest
    {
        public Guid JobId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand>
    {
        private readonly IApiClient _apiClient;

        public UpdateJobCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var data = new PostUpdateJobApiRequest.PostUpdateJobApiRequestData
            {
                CandidateId = request.CandidateId,
                EmployerName = request.EmployerName,
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                JobTitle = request.JobTitle,
                JobDescription = request.JobDescription
            };

            var apiRequest = new PostUpdateJobApiRequest(request.ApplicationId, request.JobId, data);

            await _apiClient.PostWithResponseCode(apiRequest);
        }
    }
}
