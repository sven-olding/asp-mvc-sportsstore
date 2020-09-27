﻿using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    public class IdentitySmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}