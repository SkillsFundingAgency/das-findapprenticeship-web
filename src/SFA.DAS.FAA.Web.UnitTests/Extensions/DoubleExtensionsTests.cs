using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.UnitTests.Extensions;

public class DoubleExtensionsTests
{
    [Test]
    public void ToGeoDecimalWithMetreAccuracy_Should_Truncate_To_5_Decimal_Places()
    {
        // arrange
        const double value = 10.0292321d;
        
        // act
        var result = value.ToGeoDecimalWithMetreAccuracy();
        
        // assert
        result.Should().Be(10.02923m);
    }
    
    [Test]
    public void ToGeoDecimalWithMetreAccuracy_Should_Not_Round()
    {
        // arrange
        const double value = 10.000009d;
        
        // act
        var result = value.ToGeoDecimalWithMetreAccuracy();
        
        // assert
        result.Should().Be(10m);
    }
}