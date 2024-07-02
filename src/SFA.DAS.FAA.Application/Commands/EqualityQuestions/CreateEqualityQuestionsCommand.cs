using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.EqualityQuestions
{
    public class CreateEqualityQuestionsCommand : IRequest<CreateEqualityQuestionsCommandResult>
    {
        public Guid CandidateId { get; set; }
        public GenderIdentity? Sex { get; set; }
        public EthnicGroup? EthnicGroup { get; set; }
        public EthnicSubGroup? EthnicSubGroup { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }
    }
}
