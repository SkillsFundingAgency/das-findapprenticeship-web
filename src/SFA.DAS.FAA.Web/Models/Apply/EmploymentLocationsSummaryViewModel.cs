using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Models.Apply;

public record EmploymentLocationsSummaryViewModel
{
    public List<AddressDto> Addresses { get; set; } = [];
    public Guid ApplicationId { get; set; }
    public string? BackLinkUrl { get; set; }

    [BindProperty]
    public bool? IsSectionCompleted { get; set; }

    public static implicit operator EmploymentLocationsSummaryViewModel(GetEmploymentLocationsQueryResult result)
    {
        if (result?.EmploymentLocation?.Addresses == null)
        {
            return new EmploymentLocationsSummaryViewModel();
        }

        var addresses = result.EmploymentLocation.Addresses
            .Select(x =>
            {
                Address? employmentAddress;
                try
                {
                    employmentAddress = !string.IsNullOrWhiteSpace(x.FullAddress)
                        ? JsonConvert.DeserializeObject<Address>(x.FullAddress)
                        : null;
                }
                catch (JsonException)
                {
                    employmentAddress = Address.Empty;
                }

                return new AddressDto
                {
                    Id = x.Id,
                    EmploymentAddress = employmentAddress,
                    IsSelected = x.IsSelected,
                    AddressOrder = x.AddressOrder
                };
            })
            .OrderBy(add => add.AddressOrder)
            .ToList();

        return new EmploymentLocationsSummaryViewModel
        {
            Addresses = addresses
        };
    }

    public record AddressDto
    {
        public Guid Id { get; init; }
        public Address? EmploymentAddress { get; init; }
        public string? FullAddress => EmploymentAddress?.ToSingleLineFullAddress();
        public bool IsSelected { get; init; }
        public short AddressOrder { get; init; }
    }
}