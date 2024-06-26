﻿using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.PhoneNumber;
public class UpdatePhoneNumberCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
