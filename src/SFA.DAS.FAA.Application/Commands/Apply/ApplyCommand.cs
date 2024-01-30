using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Vacancies.VacancyDetails;

namespace SFA.DAS.FAA.Application.Commands.Apply
{
    public class ApplyCommand : IRequest<ApplyCommandResponse>
    {
        public Guid CandidateId { get; set; }
        public string VacancyReference { get; set; }
    }

    public class ApplyCommandResponse
    {
        public Guid ApplicationId { get; set; }
    }

    public class ApplyCommandHandler(IApiClient apiClient) : IRequestHandler<ApplyCommand, ApplyCommandResponse>
    {
        public async Task<ApplyCommandResponse> Handle(ApplyCommand request, CancellationToken cancellationToken)
        {
            var apiRequestBody = new PostApplicationDetailsApiRequest.RequestBody(request.CandidateId);
            var apiRequest = new PostApplicationDetailsApiRequest(request.CandidateId, request.VacancyReference, apiRequestBody);

            var response = await apiClient.PostWithResponseCode<PostApplicationDetailsApiResponse>(apiRequest);

            return new ApplyCommandResponse
            {
                ApplicationId = response.ApplicationId
            };
        }
    }
}
