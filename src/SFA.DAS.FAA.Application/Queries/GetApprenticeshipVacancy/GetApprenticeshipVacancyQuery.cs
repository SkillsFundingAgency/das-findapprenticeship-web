using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQuery : IRequest<GetApprenticeshipVacancyQueryResult>
    {
        [Required]
        public string VacancyReference { get; set; }
    }
}
