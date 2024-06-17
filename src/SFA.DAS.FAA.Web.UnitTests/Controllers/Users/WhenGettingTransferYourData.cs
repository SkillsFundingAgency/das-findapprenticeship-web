using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    [TestFixture]
    public class WhenGettingTransferYourData
    {
        [Test, MoqAutoData]
        public void Then_View_Is_Returned(
        [Greedy] UserController controller)
        {
            var result = controller.TransferYourData();

            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
    }
}
