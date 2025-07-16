using SFA.DAS.FAA.Domain.Extensions;

namespace SFA.DAS.FAA.Domain.UnitTests.Extensions;

[TestFixture]
internal class WhenGettingClosingDate
{
    [Test]
    public void ToGdsDateString_ReturnsExpectedFormat()
    {
        var date = new DateTime(2024, 6, 1);
        var result = date.ToGdsDateString();
        result.Should().Be("1 June 2024");
    }

    [Test]
    public void ToGdsDateStringWithDayOfWeek_ReturnsExpectedFormat()
    {
        var date = new DateTime(2024, 6, 1); // Saturday
        var result = date.ToGdsDateStringWithDayOfWeek();
        result.Should().Be("Saturday 1 June 2024");
    }

    [TestCase(2024, 1, 1, "GMT")] // Winter
    [TestCase(2024, 6, 1, "BST")] // Summer
    public void ToUkTimeWithDstLabel_ReturnsExpectedSuffix(int year, int month, int day, string expectedSuffix)
    {
        var date = new DateTime(year, month, day, 9, 30, 0, DateTimeKind.Utc);
        var result = date.ToUkTimeWithDstLabel();
        result.Should().EndWith($" {expectedSuffix}");
    }

    [Test]
    public void ToUkTimeWithDstLabel_FormatsTimeCorrectly()
    {
        var date = new DateTime(2024, 6, 1, 9, 30, 0, DateTimeKind.Utc);
        var result = date.ToUkTimeWithDstLabel();
        result.Should().Contain("10:30am on Saturday 1 June 2024 BST");
    }
}
