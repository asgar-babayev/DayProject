using DayProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayProject.DAL
{
    public class Context:IdentityDbContext<AppUser>
    {
        public Context(DbContextOptions<Context> options):base(options)
        {

        }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
