using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedVacancies;

namespace SFA.DAS.FAA.Application.Queries.GetSavedVacancies
{
    public class GetSavedVacanciesQuery : IRequest<GetSavedVacanciesQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetSavedVacanciesQueryResult
    {
        public List<SavedVacancy> SavedVacancies { get; set; } = [];

        public class SavedVacancy
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string VacancyReference { get; set; } = string.Empty;
            public string EmployerName { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
        }

        public static implicit operator GetSavedVacanciesQueryResult(GetSavedVacanciesApiResponse source)
        {
            return new GetSavedVacanciesQueryResult
            {
                SavedVacancies = source.SavedVacancies.Select(x => new SavedVacancy
                {
                    Id = x.Id,
                    Title = x.Title,
                    EmployerName = x.EmployerName,
                    VacancyReference = x.VacancyReference,
                    ClosingDate = x.ClosingDate,
                    CreatedDate = x.CreatedDate,
                    City = x.City,
                    Postcode = x.Postcode,
                }).ToList()
            };
        }
    }

    public class GetSavedVacanciesQueryHandler(IApiClient apiClient) : IRequestHandler<GetSavedVacanciesQuery, GetSavedVacanciesQueryResult>
    {
        public async Task<GetSavedVacanciesQueryResult> Handle(GetSavedVacanciesQuery request, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetSavedVacanciesApiResponse>(
                new GetSavedVacanciesApiRequest(request.CandidateId));

            return response;
        }
    }
}
