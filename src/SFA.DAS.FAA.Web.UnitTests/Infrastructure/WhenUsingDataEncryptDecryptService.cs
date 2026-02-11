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
    public void Then_Null_Is_Returned_If_Null_Passed(
        [Frozen] Mock<IDataProtector> mockProtector,
        [Frozen] Mock<IDataProtectionProvider> mockProvider,
        DataProtectorService service)
    {
        //Act
        var actual = service.DecodeData(null);
        
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
        string data,
        string secret,
        [Frozen] Mock<IDataProtector> mockProtector,
        [Frozen] Mock<IDataProtectionProvider> mockProvider,
        DataProtectorService service)
    {
        // arrange
        mockProtector
            .Setup(sut => sut.Unprotect(Encoding.UTF8.GetBytes(data)))
            .Returns(Encoding.UTF8.GetBytes(secret));
        mockProvider
            .Setup(x => x.CreateProtector("FindAnApprenticeship"))
            .Returns(mockProtector.Object);
        
        // act
        var actual = service.DecodeData(WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(data)));
        
        // assert
        actual.Should().Be(secret);
    }

    [Test, MoqAutoData]
    public void Then_The_Data_Is_Protected(
        string id,
        byte[] fakeEncodedId,
        [Frozen] Mock<IDataProtector> mockProtector,
        [Frozen] Mock<IDataProtectionProvider> mockProvider,
        DataProtectorService service)
    {
        // arrange
        mockProtector
            .Setup(c => c.Protect(Encoding.UTF8.GetBytes(id)))
            .Returns(fakeEncodedId);
        mockProvider
            .Setup(x => x.CreateProtector("FindAnApprenticeship"))
            .Returns(mockProtector.Object);
        
        // act
        var actual = service.EncodedData(id);
        
        // assert
        actual.Should().Be(WebEncoders.Base64UrlEncode(fakeEncodedId));
    }
}