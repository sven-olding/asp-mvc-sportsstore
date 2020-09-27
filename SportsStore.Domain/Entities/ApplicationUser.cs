﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Phone Number")]        
        override public string PhoneNumber { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }
}
