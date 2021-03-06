using Linkdeed.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<JobOffer> JobOffer { get; set; }
        public DbSet<Message> Message { get; set; }

        public DbSet<EmployerDescription> EmployerDescription { get; set; }

        public DbSet<UserDescription> UserDescription { get; set; }
    }
}
