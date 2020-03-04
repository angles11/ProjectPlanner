using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Models;
using System;
using System.Collections.Generic;

namespace ProjectPlanner.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed projects

            modelBuilder.Entity<Project>().HasData(new Project
            {
                Id = 1,
                Name = "Calculadora de Viaticos",
                Description = "Herramienta para calcular los viaticos de vuelo",
                Status = "Pending",
                CreatedDate = DateTime.Today,
                EstimatedDate = DateTime.Now.AddDays(5),
                Todos = new List<Todo>(),
            }); ;

            modelBuilder.Entity<Project>().HasData(new Project
            {
                Id = 2,
                Name = "Calculadora de Vencimiento",
                Description = "Herramienta para calcular los tiempos limites de vuelo",
                Status = "In Progress",
                CreatedDate = DateTime.Today,
                EstimatedDate = DateTime.Now.AddDays(10),
                Todos = new List<Todo>(),
            });
            modelBuilder.Entity<Project>().HasData(new Project
            {
                Id = 3,
                Name = "Todo List",
                Description = "Herramienta para llevar el control de los proyectos",
                Status = "Completed",
                CreatedDate = DateTime.Today,
                EstimatedDate = DateTime.Now.AddDays(30),
                Todos = new List<Todo>(),
            });
        }
    }
}
