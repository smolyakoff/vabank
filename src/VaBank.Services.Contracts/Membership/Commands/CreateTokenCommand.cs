﻿using System;

namespace VaBank.Services.Contracts.Membership.Commands
{
    public class CreateTokenCommand
    {
        public string Id { get; set; }
        public string ProtectedTicket { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public Guid UserId { get; set; }
    }
}
