using SFA.DAS.FAA.Domain.Interfaces;
using System;
using System.Collections.Generic;
namespace SFA.DAS.FAA.Domain.User
{
    public class UpdateNameApiRequest : IPutApiRequest
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
        public string PutUrl => $"/users/{_candidateId}/add-details";
    }
}
