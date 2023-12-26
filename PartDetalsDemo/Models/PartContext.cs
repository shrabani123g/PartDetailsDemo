using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using Dapper;
//using System.Data.SqlClient;

namespace PartDetailsDemo.Models
{
    public class PartContext : DbContext
    {
        public PartContext(DbContextOptions<PartContext> options) : base(options)
        {
        }

        public DbSet<Part> Part { get; set; }
        public DbSet<User> User { get; set; }
    }
}
