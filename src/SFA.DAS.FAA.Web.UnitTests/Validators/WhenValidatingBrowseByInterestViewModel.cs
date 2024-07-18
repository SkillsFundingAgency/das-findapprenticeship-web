using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators
{
    public class WhenValidatingBrowseByInterestViewModel
    {
        [TestCase(null, false)]
        public async Task Then_The_What_Field_Is_Validated(List<int>? inputValue, bool isValid)
        {
            var model = new BrowseByInterestViewModel
            {
                SelectedRouteIds = inputValue
            };

            var validator = new BrowseByInterestViewModelValidator();

            var actual = await validator.ValidateAsync(model);

            actual.IsValid.Should().Be(isValid);
        }
    }
}
