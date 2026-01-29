using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators
{
    public class WhenValidatingLocationViewModel
    {
        private const string NoNationalSearchErrorMessage = "Select a search location";

        [Test]
        [MoqInlineAutoData(true, "", true)]
        [MoqInlineAutoData(false, "", true)]
        [MoqInlineAutoData(null, NoNationalSearchErrorMessage, false)]
        public async Task And_FirstName_Invalid_ValidationResult_Returned(bool? nationalSearch, string? errorMessage, bool isValid)
        {
            var model = new LocationViewModel
            {
                NationalSearch = nationalSearch
            };

            var sut = new LocationViewModelValidator();
            var result = await sut.TestValidateAsync(model);

            if (!isValid)
            {
                result.ShouldHaveValidationErrorFor(c => c.NationalSearch)
                    .WithErrorMessage(errorMessage);
            }
            else
            {
                result.ShouldNotHaveValidationErrorFor(c => c.NationalSearch);
            }
        }
    }
}
