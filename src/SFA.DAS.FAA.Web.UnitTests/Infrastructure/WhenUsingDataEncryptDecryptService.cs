using System.Security.Cryptography;
using System.Text;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Infrastructure;

public class WhenUsingDataEncryptDecryptService
{
    [Test, MoqAutoData]
    public void Then_Null_Is_Returned_If_Cryptographic_Exception_Thrown(
        string data,
        [Frozen] Mock<IDataProtector> mockProtector,
        [Frozen] Mock<IDataProtectionProvider> mockProvider,
        DataProtectorService service)
    {
        //Arrange
        mockProtector
            .Setup(sut => sut.Unprotect(It.IsAny<byte[]>()))
            .Throws<CryptographicException>();
        mockProvider
            .Setup(x => x.CreateProtector("FindAnApprenticeship"))
            .Returns(mockProtector.Object);
        
        //Act
        var actual = service.DecodeData(data);
        
        //Assert
        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public void Then_If_Incorrect_Format_Then_Exception_Thrown_And_Null_Returned(
        string id,
        [Frozen] Mock<IDataProtector> mockProtector,
        [Frozen] Mock<IDataProtectionProvider> mockProvider,
        DataProtectorService service)
    {
        //Act
        var actual = service.DecodeData("&^%$");
        
        //Assert
        actual.Should().BeNull();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Valid_Then_Id_Returned(
        string id,
        [Frozen] Mock<IDataProtector> mockProtector,
        [Frozen] Mock<IDataProtectionProvider> mockProvider,
        DataProtectorService service)
    {
        var encodedData = Encoding.UTF8.GetBytes(id);
        var data = WebEncoders.Base64UrlEncode(encodedData);
        var toEncode = WebEncoders.Base64UrlDecode(id);
        mockProtector.Setup(c => c.Protect(It.Is<byte[]>(
            x => x[0].Equals(Encoding.UTF8.GetBytes($"{id}")[0])))).Returns(toEncode);
        mockProtector
            .Setup(sut => sut.Unprotect(It.IsAny<byte[]>()))
            .Returns(encodedData);
        mockProvider
            .Setup(x => x.CreateProtector("FindAnApprenticeship"))
            .Returns(mockProtector.Object);
        
        //Act
        var actual = service.DecodeData(data);
        
        //Assert
        actual.Should().Be(id);
    }

    [Test, MoqAutoData]
    public void Then_The_Data_Is_Protected(
        string id,
        [Frozen] Mock<IDataProtector> mockProtector,
        [Frozen] Mock<IDataProtectionProvider> mockProvider,
        DataProtectorService service)
    {
        //Arrange
        var toEncode = WebEncoders.Base64UrlDecode(id.ToString());
        mockProtector.Setup(c => c.Protect(It.Is<byte[]>(
            x => x[0].Equals(Encoding.UTF8.GetBytes($"{id}")[0])))).Returns(toEncode);
        mockProvider
            .Setup(x => x.CreateProtector("FindAnApprenticeship"))
            .Returns(mockProtector.Object);
        
        //Act
        var actual = service.EncodedData(id);
        
        //Assert
        Assert.That(WebEncoders.Base64UrlEncode(toEncode), Is.EqualTo(actual));
    }
}