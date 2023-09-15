using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using MediatR;
using SFA.DAS.FAA.Application.Vacancies.Queries;

namespace SFA.DAS.FAA.Web.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController
    (IMediator mediator
    )
    {
        _mediator = mediator;
    }
        
    
    [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public async Task<IActionResult> Index(GetVacanciesRequest request )
    {
        var result = await _mediator.Send(new GetVacanciesQuery());

        var viewModel = new VacanciesViewModel
        {
            Total = result.Total
        };

        return View(viewModel);
    }
}

