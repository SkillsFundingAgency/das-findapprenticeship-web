﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingConfirmYourAccountDetails
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        string govIdentifier,
        string candidateId,
        string phoneNumber,
        GetCandidateAccountDetailsQueryResult queryResult,
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
                        new Claim(CustomClaims.CandidateId, candidateId),
                        new Claim(ClaimTypes.MobilePhone, phoneNumber)
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidateAccountDetailsQuery>(x => x.GovUkIdentifier == govIdentifier), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.ConfirmYourAccountDetails() as ViewResult;
        var resultModel = result.Model as ConfirmAccountDetailsViewModel;

        resultModel.Postcode.Should().BeEquivalentTo(queryResult.Postcode);
    }
}