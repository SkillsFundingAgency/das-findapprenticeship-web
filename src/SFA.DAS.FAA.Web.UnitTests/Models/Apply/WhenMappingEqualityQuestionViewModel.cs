using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply
{
    public class WhenMappingEqualityQuestionViewModel
    {
        [Test]
        [MoqInlineAutoData(GenderIdentity.Female)]
        [MoqInlineAutoData(GenderIdentity.Male)]
        [MoqInlineAutoData(GenderIdentity.PreferNotToSay)]
        public void Map_EqualityQuestionsGenderViewModel_Returns_Expected_Result(GenderIdentity gender, EqualityQuestionsGenderViewModel source)
        {
            source.Sex = gender.ToString();
            var result = new EqualityQuestionsModel();
            result.Apply(source);

            using (new AssertionScope())
            {
                result.IsGenderIdentifySameSexAtBirth.Should().Be(source.IsGenderIdentifySameSexAtBirth);
                result.Sex.Should().Be(gender);
            }
        }
    }
}
