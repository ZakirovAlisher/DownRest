using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DownRest.Models
{
    public class Context: DbContext
    {
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Skill> Skills { get; set; }

       public DbSet<Client> Clients { get; set; }
       public DbSet<Project> Projects { get; set; }

       public DbSet<Response> Responses { get; set; }
      
        public DbSet<Category> Categories { get; set; }

     
       
        public Context() : base("Context")
        { }

       protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Context>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        
        modelBuilder.Entity<Skill>().HasMany(c => c.Users)
              .WithMany(s => s.Skills)
                .Map(t => t.MapLeftKey("Skill_Id")
               .MapRightKey("User_Id")
                .ToTable("UsersSkills"));
    }
    }
}