using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Todo> Todos { get; set; }
    }
}
