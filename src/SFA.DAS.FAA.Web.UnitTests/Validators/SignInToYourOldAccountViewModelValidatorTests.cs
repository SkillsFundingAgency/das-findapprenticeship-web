using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators
{
    [TestFixture]
    public class SignInToYourOldAccountViewModelValidatorTests
    {
        [Test, MoqAutoData]
        public void When_Email_And_Password_Are_Supplied_Then_Is_Valid(SignInToYourOldAccountViewModelValidator validator)
        {
            var viewModel = new SignInToYourOldAccountViewModel
            {
                Email = "test@test.com",
                Password = "secret"
            };

            var result = validator.Validate(viewModel);

            result.IsValid.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public void When_Email_Is_Not_Supplied_Then_Is_Invalid(SignInToYourOldAccountViewModelValidator validator)
        {
            var viewModel = new SignInToYourOldAccountViewModel
            {
                Password = "secret"
            };

            var result = validator.Validate(viewModel);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Email");
        }

        [Test, MoqAutoData]
        public void When_Email_Is_Not_In_The_Correct_Format_Then_Is_Invalid(SignInToYourOldAccountViewModelValidator validator)
        {
            var viewModel = new SignInToYourOldAccountViewModel
            {
                Email = "wrong",
                Password = "secret"
            };

            var result = validator.Validate(viewModel);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Email");
        }

        [Test, MoqAutoData]
        public void When_Password_Is_Not_Supplied_Then_Is_Invalid(SignInToYourOldAccountViewModelValidator validator)
        {
            var viewModel = new SignInToYourOldAccountViewModel
            {
                Email = "test@test.com",
            };

            var result = validator.Validate(viewModel);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Password");
        }
    }
}
