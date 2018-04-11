﻿using CafeT.Frameworks.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmartTracking.Stores
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string,
        ApplicationUserLogin, ApplicationUserRole,
        ApplicationUserClaim>, IUserStore<ApplicationUser, string>,
        IDisposable
    {
        public ApplicationUserStore() : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }


        public ApplicationUserStore(DbContext context)
            : base(context)
        {

        }
    }
}