using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators
{
    public class WhenValidatingNameViewModel
    {
        private const string FirstNameErrorMessage = "Enter your first name";
        private const string LastNameErrorMessage = "Enter your last name";

        [Test]
        [MoqInlineAutoData(null, FirstNameErrorMessage, false)]
        [MoqInlineAutoData("", FirstNameErrorMessage, false)]
        [MoqInlineAutoData("some name", null, true)]
        public async Task And_FirstName_Invalid_ValidationResult_Returned(string? firstName, string? errorMessage, bool isValid)
        {
            var model = new NameViewModel
            {
                FirstName = firstName
            };

            var sut = new NameViewModelValidator();
            var result = await sut.TestValidateAsync(model);

            if (!isValid)
            {
                result.ShouldHaveValidationErrorFor(c => c.FirstName)
                    .WithErrorMessage(errorMessage);
            }
            else
            {
                result.ShouldNotHaveValidationErrorFor(c => c.FirstName);
            }
        }

        [Test]
        [MoqInlineAutoData(null, LastNameErrorMessage, false)]
        [MoqInlineAutoData("", LastNameErrorMessage, false)]
        [MoqInlineAutoData("some name", null, true)]
        public async Task And_LastName_Invalid_ValidationResult_Returned(string? lastName, string? errorMessage, bool isValid)
        {
            var model = new NameViewModel
            {
                LastName = lastName
            };

            var sut = new NameViewModelValidator();
            var result = await sut.TestValidateAsync(model);

            if (!isValid)
            {
                result.ShouldHaveValidationErrorFor(c => c.LastName)
                    .WithErrorMessage(errorMessage);
            }
            else
            {
                result.ShouldNotHaveValidationErrorFor(c => c.LastName);
            }
        }
    }
}
