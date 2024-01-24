using SFA.DAS.FAA.Domain.Interfaces;
using System;
using System.Collections.Generic;
namespace SFA.DAS.FAA.Domain.User
{
    public record UpdateNameApiRequest(string FirstName, string LastName) : IPostApiRequest//get sarahs branch for put req 
    {
        public object Data { get; set; } //I'm not sure about this 
        public string PostUrl => $"/users/{candidateId}/add-details?firstName={FirstName} lastName={LastName}";
    }
}
