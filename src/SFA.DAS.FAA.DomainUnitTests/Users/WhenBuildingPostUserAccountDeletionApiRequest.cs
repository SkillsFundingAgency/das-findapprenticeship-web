﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users
{
    [TestFixture]
    public class WhenBuildingPostUserAccountDeletionApiRequest
    {
        [Test, AutoData]
        public void Then_Then_Request_Is_Built(Guid candidateId)
        {
            var actual = new PostUserAccountDeletionApiRequest(candidateId);

            actual.PostUrl.Should().Be($"users/{candidateId}/account-deletion");
        }
    }
}