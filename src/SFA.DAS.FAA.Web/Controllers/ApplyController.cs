﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("vacancies/{vacancyReference}/apply", Name = RouteNames.Apply)]
    public class ApplyController(IMediator mediator, IDateTimeService dateTimeService) : Controller
    {
        public async Task<IActionResult> Index(GetIndexRequest request)
        {
            var query = new GetIndexQuery { VacancyReference = request.VacancyReference };
            var result = await mediator.Send(query);
            var viewModel = IndexViewModel.Map(dateTimeService, result);
            return View(viewModel);
        }
    }
}