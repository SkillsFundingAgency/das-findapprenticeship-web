using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Models
{
    public record EmploymentLocationViewModel
    {
        public Guid Id { get; init; }
        private Address? EmploymentAddress { get; init; }
        public string? FullAddress => EmploymentAddress?.ToSingleLineFullAddress();
        public bool IsSelected { get; init; }
        public short AddressOrder { get; init; }

        public static implicit operator EmploymentLocationViewModel(AddressDto addressDto)
        {
            Address? employmentAddress;
            try
            {
                employmentAddress = !string.IsNullOrWhiteSpace(addressDto.FullAddress)
                    ? JsonConvert.DeserializeObject<Address>(addressDto.FullAddress)
                    : null;
            }
            catch (JsonException)
            {
                employmentAddress = Address.Empty;
            }

            return new EmploymentLocationViewModel
            {
                Id = addressDto.Id,
                EmploymentAddress = employmentAddress,
                IsSelected = addressDto.IsSelected,
                AddressOrder = addressDto.AddressOrder
            };
        }
    }
}
