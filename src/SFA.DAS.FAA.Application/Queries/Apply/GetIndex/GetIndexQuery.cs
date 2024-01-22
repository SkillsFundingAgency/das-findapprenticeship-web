using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetIndex
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public string VacancyReference { get; set; }
        public string ApplicantEmailAddress { get; set; }
    }
}
