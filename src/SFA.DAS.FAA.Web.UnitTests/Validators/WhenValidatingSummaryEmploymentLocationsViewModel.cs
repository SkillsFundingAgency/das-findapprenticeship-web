using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators
{
    [TestFixture]
    public class WhenValidatingSummaryEmploymentLocationsViewModel
    {
        [Test]
        public async Task Then_Validation_Should_Fail_When_No_Option_Selected()
        {
            var model = new EmploymentLocationsSummaryViewModel
            {
                IsSectionCompleted = null
            };
            var validator = new EmploymentLocationsSummaryViewModelValidator();
            var result = await validator.TestValidateAsync(model);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Select if you have completed this section");
        }

        [Test]
        public async Task Then_Validation_Should_Pass_When_Given_IsSectionCompleted()
        {
            var model = new EmploymentLocationsSummaryViewModel
            {
                IsSectionCompleted = true
            };
            var validator = new EmploymentLocationsSummaryViewModelValidator();
            var result = await validator.TestValidateAsync(model);
            result.IsValid.Should().BeTrue();
        }
    }
}
