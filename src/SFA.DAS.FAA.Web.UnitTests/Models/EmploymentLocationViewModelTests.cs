using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models
{
    [TestFixture]
    public class EmploymentLocationViewModelTests
    {
        [Test]
        public void ImplicitOperator_ShouldMapProperties_WhenFullAddressIsValidJson()
        {
            // Arrange
            var address = new Address("Line1", "Line2", "Line3", "Line4", "AB12 3CD", 1.23, 4.56);
            var addressJson = JsonConvert.SerializeObject(address);
            var dto = new AddressDto
            {
                Id = Guid.NewGuid(),
                FullAddress = addressJson,
                IsSelected = true,
                AddressOrder = 2
            };

            // Act
            EmploymentLocationViewModel viewModel = dto;

            // Assert
            viewModel.Id.Should().Be(dto.Id);
            viewModel.FullAddress.Should().Be(address.ToSingleLineFullAddress());
            viewModel.IsSelected.Should().BeTrue();
            viewModel.AddressOrder.Should().Be(dto.AddressOrder);
        }

        [Test]
        public void ImplicitOperator_ShouldSetEmploymentAddressToNull_WhenFullAddressIsNullOrWhitespace()
        {
            // Arrange
            var dto = new AddressDto
            {
                Id = Guid.NewGuid(),
                FullAddress = " ",
                IsSelected = false,
                AddressOrder = 1
            };

            // Act
            EmploymentLocationViewModel viewModel = dto;

            // Assert
            viewModel.Id.Should().Be(dto.Id);
            viewModel.FullAddress.Should().BeNullOrEmpty();
            viewModel.IsSelected.Should().BeFalse();
            viewModel.AddressOrder.Should().Be(dto.AddressOrder);
        }

        [Test]
        public void ImplicitOperator_ShouldSetEmploymentAddressToEmpty_WhenFullAddressIsInvalidJson()
        {
            // Arrange
            var dto = new AddressDto
            {
                Id = Guid.NewGuid(),
                FullAddress = "not a json",
                IsSelected = true,
                AddressOrder = 3
            };

            // Act
            EmploymentLocationViewModel viewModel = dto;

            // Assert
            viewModel.Id.Should().Be(dto.Id);
            viewModel.FullAddress.Should().Be(Address.Empty.ToSingleLineFullAddress());
            viewModel.IsSelected.Should().BeTrue();
            viewModel.AddressOrder.Should().Be(dto.AddressOrder);
        }
    }
}
