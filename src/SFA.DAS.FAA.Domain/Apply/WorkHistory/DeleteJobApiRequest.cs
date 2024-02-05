using SFA.DAS.FAA.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory
{
    public class DeleteJobApiRequest : IDeleteApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        private readonly Guid _workHistoryId;

        public DeleteJobApiRequest(Guid applicationId, Guid candidateId, Guid workHistoryId)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            _workHistoryId = workHistoryId;
        }

        public string DeleteUrl => $"candidates/{_candidateId}/applications/{_applicationId}/work-history/{_workHistoryId}";
    }
}
