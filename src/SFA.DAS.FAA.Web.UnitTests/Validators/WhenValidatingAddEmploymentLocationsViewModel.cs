using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators
{
    [TestFixture]
    public class WhenValidatingAddEmploymentLocationsViewModel
    {
        [Test]
        public async Task Then_Validation_Should_Fail_When_No_Addresses_Selected()
        {
            var model = new AddEmploymentLocationsViewModel
            {
                SelectedAddressIds = []
            };
            var validator = new AddEmploymentLocationsViewModelValidator();
            var result = await validator.TestValidateAsync(model);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Select where you want to apply for");
        }

        [Test]
        public async Task Then_Validation_Should_Pass_When_At_Least_One_Address_Selected()
        {
            var model = new AddEmploymentLocationsViewModel
            {
                SelectedAddressIds = [ Guid.NewGuid() ]
            };
            var validator = new AddEmploymentLocationsViewModelValidator();
            var result = await validator.TestValidateAsync(model);
            result.IsValid.Should().BeTrue();
        }
    }
}
