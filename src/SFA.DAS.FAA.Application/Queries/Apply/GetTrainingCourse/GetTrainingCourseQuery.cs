using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse
{
    public class GetTrainingCourseQuery : IRequest<GetTrainingCourseQueryResult>
    {
        public Guid CandidateId { get; init; }
        public Guid ApplicationId { get; init; }
        public Guid TrainingCourseId { get; set; }
    }

    public class GetTrainingCourseQueryResult
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }

        public static implicit operator GetTrainingCourseQueryResult(GetTrainingCourseApiResponse source)
        {
            return new GetTrainingCourseQueryResult
            {
                Id = source.Id,
                ApplicationId = source.ApplicationId,
                CourseName = source.CourseName,
                YearAchieved = source.YearAchieved
            };
        }
    }

    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery, GetTrainingCourseQueryResult>
    {
        private readonly IApiClient _apiClient;

        public GetTrainingCourseQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetTrainingCourseQueryResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Get<GetTrainingCourseApiResponse>(
                new GetTrainingCourseApiRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));

            return response;
        }
    }

}
