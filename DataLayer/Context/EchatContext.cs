using DataLayer.Entities.Chats;
using DataLayer.Entities.Roles;
using DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class EchatContext : DbContext
    {
        public EchatContext(DbContextOptions<EchatContext> options) : base(options) { }

        #region Entities
        public DbSet<UserChat> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        #endregion

        #region Relations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChat>()
                .HasMany(b => b.Chats)
                .WithOne(c => c.UserChat)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserChat>()
                .HasMany(c=>c.UserGroups)
                .WithOne(v=>v.UserChat)
                .HasForeignKey(b => b.userId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatGroup>()
                .HasMany(c => c.UserGroups)
                .WithOne(v => v.ChatGroup)
                .HasForeignKey(b => b.groupId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserChat>()
                .HasMany(x=> x.Groups)
                .WithOne(c=>c.User)
                .HasForeignKey(x=>x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
        #endregion
        
    }
}
