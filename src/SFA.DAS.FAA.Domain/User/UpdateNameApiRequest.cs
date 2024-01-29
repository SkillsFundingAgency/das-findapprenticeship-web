using SFA.DAS.FAA.Domain.Interfaces;
using System;
using System.Collections.Generic;
namespace SFA.DAS.FAA.Domain.User
{
    public class UpdateNameApiRequest : IPutApiRequest
    {
        private readonly string _govIdentifier;

        public UpdateNameApiRequest(string firstName, string lastName, string govIdentifier, string email)
        {
            _govIdentifier = govIdentifier;
            Data = new 
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };
        }
        public object Data { get; set; }
        public string PutUrl => $"/users/{_govIdentifier}/add-details";
    }
}
