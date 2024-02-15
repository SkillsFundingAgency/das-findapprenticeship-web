using MediatR;
using SFA.DAS.FAA.Domain.Apply.AddTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.TrainingCourses.AddTrainingCourse
{
    public class AddTrainingCourseCommand : IRequest<AddTrainingCourseCommandResponse>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }

    public class AddTrainingCourseCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class AddTrainingCourseCommandHandler : IRequestHandler<AddTrainingCourseCommand, AddTrainingCourseCommandResponse>
    {
        private readonly IApiClient _apiClient;

        public AddTrainingCourseCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AddTrainingCourseCommandResponse> Handle(AddTrainingCourseCommand request, CancellationToken cancellationToken)
        {
            var data = new PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData
            {
                CandidateId = request.CandidateId,
                CourseName = request.CourseName,
                YearAchieved = request.YearAchieved
            };

            var apiRequest = new PostTrainingCourseApiRequest(request.ApplicationId, data);

            var apiResponse = await _apiClient.PostWithResponseCode<Guid>(apiRequest);

            return new AddTrainingCourseCommandResponse
            {
                Id = apiResponse
            };
        }
    }
}
