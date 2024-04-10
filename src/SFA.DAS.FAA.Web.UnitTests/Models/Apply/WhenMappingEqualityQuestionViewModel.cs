using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply
{
    public class WhenMappingEqualityQuestionViewModel
    {
        [Test, MoqAutoData]
        public void Map_EqualityQuestionsGenderViewModel_Returns_Expected_Result(EqualityQuestionsGenderViewModel source)
        {
            var result = (EqualityQuestionsModel)source;

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(source, options => options
                    .Excluding(ex => ex.ErrorDictionary)
                    .Excluding(ex => ex.Valid)
                );
            }
        }
    }
}
