using SFA.DAS.FAA.Domain.Interfaces;
using System;
using System.Collections.Generic;
namespace SFA.DAS.FAA.Domain.User
{
    public class UpdateNameApiRequest : IPostApiRequest//get sarahs branch for put req 
    {
        private readonly Guid _candidateId;

        public UpdateNameApiRequest(string firstName, string lastName, Guid candidateId)
        {
            _candidateId = candidateId;
            Data = new 
            {
                FirstName = firstName,
                LastName = lastName
            };
        }
        public object Data { get; set; }
        public string PostUrl => $"/users/{_candidateId}/add-details";
    }
}
