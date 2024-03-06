using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators
{
    [TestFixture]
    public class WhenValidatingWhatInterestsYouViewModel
    {
        [Test]
        public async Task Then_IsSectionCompleted_Is_Mandatory()
        {
            var validator = new WhatInterestsYouViewModelValidator();

            var model = new WhatInterestsYouViewModel
            {
                ApplicationId = Guid.NewGuid(),
                IsSectionCompleted = null
            };

            var result = await validator.TestValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.IsSectionCompleted).WithErrorMessage("Select if you have finished this section");
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public async Task Then_If_SectionCompleted_Then_AnswerText_Is_Mandatory(bool? isSectionCompleted, bool expectError)
        {
            var validator = new WhatInterestsYouViewModelValidator();

            var model = new WhatInterestsYouViewModel
            {
                ApplicationId = Guid.NewGuid(),
                IsSectionCompleted = isSectionCompleted,
                AnswerText = string.Empty
            };

            var result = await validator.TestValidateAsync(model);

            result.IsValid.Should().Be(!expectError);
            if (!result.IsValid)
            {
                result.ShouldHaveValidationErrorFor(x => x.AnswerText).WithErrorMessage("Enter your interest in this apprenticeship – you must enter an answer before making this section complete");
            }
        }

        [TestCase(300, false)]
        [TestCase(301, true)]
        public async Task Then_AnswerText_Cannot_Exceed_300_Words(int wordCount, bool expectError)
        {
            var validator = new WhatInterestsYouViewModelValidator();

            var model = new WhatInterestsYouViewModel
            {
                ApplicationId = Guid.NewGuid(),
                IsSectionCompleted = true,
                AnswerText = string.Concat(Enumerable.Repeat("x ", wordCount))
            };

            var result = await validator.TestValidateAsync(model);

            result.IsValid.Should().Be(!expectError);

            if (!result.IsValid)
            {
                result.ShouldHaveValidationErrorFor(x => x.AnswerText).WithErrorMessage("Your interest in this apprenticeship must be 300 words or less");
            }
        }
    }
}
