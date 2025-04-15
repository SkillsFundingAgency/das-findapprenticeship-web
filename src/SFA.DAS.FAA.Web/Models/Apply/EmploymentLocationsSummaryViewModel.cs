using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.Models.Apply;

public record EmploymentLocationsSummaryViewModel
{
    public List<AddressDto> Addresses { get; set; } = [];
    public Guid ApplicationId { get; set; }
    public string? BackLinkUrl { get; init; }

    [BindProperty]
    public bool? IsSectionCompleted { get; set; }
}