using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingPostcodeAddress
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        string email,
        Guid candidateId,
        string govIdentifier,
        string? postcode,
        GetCandidatePostcodeQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(CustomClaims.CandidateId, candidateId)
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId.ToString() == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = controller.PostcodeAddress(postcode) as ViewResult;

        result.Should().NotBeNull();
    }
}
