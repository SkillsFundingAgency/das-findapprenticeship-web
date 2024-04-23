using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingSubjectViewModel
{
    [Test]
    public async Task Then_Not_Validated_If_Deleted()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = true
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("apprenticeship", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeTrue();
    }
    [Test]
    public async Task Then_Not_Validated_If_Deleted_Is_Null()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = null
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("apprenticeship", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeTrue();
    }
    [Test]
    public async Task Then_The_Apprenticeship_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("apprenticeship", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(2);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.AdditionalInformationErrorMessage)
            .Should().NotBeNull();
    }
    [TestCase("","additionalInfo", false)]
    [TestCase("name","", false)]
    [TestCase("name","additionalInfo", true)]
    public async Task Then_The_Apprenticeship_Qualification_Is_Validated_For_New(string name, string additionalInformation, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            AdditionalInformation = additionalInformation,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("apprenticeship", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(1);
        }
    }
    
    [Test]
    public async Task Then_The_Btec_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Grade = string.Empty,
            Level = "select",
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("btec", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(3);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.AdditionalInformationErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.GradeErrorMessage)
            .Should().NotBeNull();
    }
    
    [TestCase("","value","", false)]
    [TestCase("name","select","", false)]
    [TestCase("","select","grade", false)]
    [TestCase("name","additionalInfo","grade", true)]
    public async Task Then_The_Btec_Qualification_Is_Validated_For_New(string name, string level,string grade, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            Level = level,
            Grade = grade,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("btec", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(2);
        }
    }
    
    [Test]
    public async Task Then_The_Degree_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("degree", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(3);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.AdditionalInformationErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.GradeErrorMessage)
            .Should().NotBeNull();
    }
    
    [TestCase("","additionalInfo","", false)]
    [TestCase("name","","", false)]
    [TestCase("","","grade", false)]
    [TestCase("name","additionalInfo","grade", true)]
    public async Task Then_The_Degree_Qualification_Is_Validated_For_New(string name, string additionalInformation,string grade, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            AdditionalInformation = additionalInformation,
            Grade = grade,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("degree", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(2);
        }
    }
    
    [Test]
    public async Task Then_The_A_Level_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("a Level", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(2);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.GradeErrorMessage)
            .Should().NotBeNull();
    }
    
    [TestCase("name","", false)]
    [TestCase("","grade", false)]
    [TestCase("name","grade", true)]
    public async Task Then_The_A_Level_Qualification_Is_Validated_For_New(string name, string grade, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            Grade = grade,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("a level", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(1);
        }
    }
    
    [Test]
    public async Task Then_The_T_Level_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("T Level", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(2);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.GradeErrorMessage)
            .Should().NotBeNull();
    }
    
    [TestCase("name","", false)]
    [TestCase("","grade", false)]
    [TestCase("name","grade", true)]
    public async Task Then_The_T_Level_Qualification_Is_Validated_For_New(string name, string grade, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            Grade = grade,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("T level", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(1);
        }
    }
    
    [Test]
    public async Task Then_The_AS_Level_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("AS Level", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(2);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.GradeErrorMessage)
            .Should().NotBeNull();
    }
    
    [TestCase("name","", false)]
    [TestCase("","grade", false)]
    [TestCase("name","grade", true)]
    public async Task Then_The_AS_Level_Qualification_Is_Validated_For_New(string name, string grade, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            Grade = grade,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("as level", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(1);
        }
    }
    
    [Test]
    public async Task Then_The_GCSE_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("GCSE", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(2);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.GradeErrorMessage)
            .Should().NotBeNull();
    }
    
    [TestCase("name","", false)]
    [TestCase("","grade", false)]
    [TestCase("name","grade", true)]
    public async Task Then_The_Gcse_Qualification_Is_Validated_For_New(string name, string grade, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            Grade = grade,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("gcse", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(1);
        }
    }
    
    [Test]
    public async Task Then_The_Other_Qualification_Is_Validated_For_Existing()
    {
        var model = new SubjectViewModel
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("other", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().BeFalse();
        actual.Errors.Count.Should().Be(1);
        actual.Errors.SingleOrDefault(c => c.ErrorMessage == qualificationDisplayTypeViewModel.SubjectErrorMessage)
            .Should().NotBeNull();
    }
    
    [TestCase("", false)]
    [TestCase("name", true)]
    public async Task Then_The_Other_Qualification_Is_Validated_For_New(string name, bool isValid)
    {
        var model = new SubjectViewModel
        {
            Id = null,
            Name = name,
            IsDeleted = false
        };
        var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel("Other", Guid.NewGuid());
        var validator = new SubjectViewModelValidator(qualificationDisplayTypeViewModel);

        var actual = await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.Errors.Count.Should().Be(1);
        }
    }
}