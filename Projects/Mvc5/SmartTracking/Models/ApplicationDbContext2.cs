﻿using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.DataContext;
using System.Data.Entity;
using System;
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using CafeT.Frameworks.Identity.Models;

namespace SmartTracking.Models
{
    public class ApplicationDbContext2 : IdentityDbContext<ApplicationUser, ApplicationRole,
        string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim> , IDataContextAsync
    {
        public ApplicationDbContext2() : base("DefaultConnection")
        {
        }
        //public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<NewIssue> NewIssues { get; set; }

        //public DbSet<WorkIssue> Issues { get; set; }
        //public DbSet<Document> Documents { get; set; }
        //public DbSet<Project> Projects { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        //public DbSet<Question> Questions { get; set; }
        //public DbSet<Answer> Answers { get; set; }
        //public DbSet<Story> Stories { get; set; }
        //public DbSet<Url> Urls { get; set; }
        //public DbSet<Contact> Contacts { get; set; }

        // Add the ApplicationGroups property:
        public virtual IDbSet<ApplicationGroup> ApplicationGroups { get; set; }

        public static ApplicationDbContext2 Create()
        {
            return new ApplicationDbContext2();
        }

        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationUserGroup>((ApplicationGroup g) => g.ApplicationUsers)
                .WithRequired().HasForeignKey<string>((ApplicationUserGroup ag) => ag.ApplicationGroupId);

            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey((ApplicationUserGroup r) =>
                    new
                    {
                        ApplicationUserId = r.ApplicationUserId,
                        ApplicationGroupId = r.ApplicationGroupId
                    }).ToTable("ApplicationUserGroups");

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationGroupRole>((ApplicationGroup g) => g.ApplicationRoles)
                .WithRequired().HasForeignKey<string>((ApplicationGroupRole ap) => ap.ApplicationGroupId);
            modelBuilder.Entity<ApplicationGroupRole>().HasKey((ApplicationGroupRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationGroupId = gr.ApplicationGroupId
                }).ToTable("ApplicationGroupRoles");

        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }
    }
}