using ACS.Core.Entities;
using ACS.Core.Entities.Base;
using ACS.Core.Interfaces;
using ACS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACS.Infrastructure.Data
{
    public class ACSDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        private readonly IAppContext? _appContext;

        public ACSDbContext(DbContextOptions<ACSDbContext> options) : base(options)
        {
            //TODO: Add AppContext
        }

        public ACSDbContext(DbContextOptions<ACSDbContext> options, IAppContext? appContext) : this(options)
        {
            _appContext = appContext;
        }

        public DbSet<RefreshToken>? RefreshTokens { get; set; }
        public DbSet<ACSUser>? ACSUsers { get; set; }

        public override int SaveChanges()
        {
            //AddAudit();
            return base.SaveChanges();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //AddAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<ACSUser>(ConfigureUser);
            base.OnModelCreating(model);
        }

        private void ConfigureUser(EntityTypeBuilder<ACSUser> builder)
        {
            var nav = builder.Metadata.FindNavigation(nameof(ACSUser.RefreshTokens));
            nav.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(b => b.Email);
            builder.Ignore(b => b.PasswordHash);
        }

        private void AddAudit()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();

            foreach(var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if(entry.State == EntityState.Added)
                {
                    entity.CreationDate = DateTime.UtcNow;
                    //entity.Creator = _appContext.UserName;
                    entity.Creator = "";
                }

                entity.UpdateDate = DateTime.UtcNow;
                entity.Updater = "";
                //entity.Updater = _appContext.UserName;
            }
        }
    }
}