using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingSubjectViewModel
{
    [Test]
    public async Task Then_The_Apprenticeship_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid()
        };
        var validator =
            new SubjectViewModelValidator(new QualificationDisplayTypeViewModel("apprenticeship", Guid.NewGuid()));

        var actual = await validator.ValidateAsync(model);
    }
}