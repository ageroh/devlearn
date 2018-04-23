using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Services
{
    public class SbContext : DbContext
    {
        public SbContext(DbContextOptions<SbContext> options)
            : base(options)
        {

        }
        
        public DbSet<Event> Event { get; set; }
        public DbSet<Score> Score { get; set; }
    }
}
