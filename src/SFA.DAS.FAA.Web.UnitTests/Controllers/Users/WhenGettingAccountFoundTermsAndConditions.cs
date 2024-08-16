using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    public class WhenGettingAccountFoundTermsAndConditions
    {
        [Test, MoqAutoData]
        public void Then_View_Is_Returned(
            [Greedy] UserController controller)
        {
            var result = controller.AccountFoundTermsAndConditions() as ViewResult;
            result.Should().NotBeNull();
        }
    }
}