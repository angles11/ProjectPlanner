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
                ProjectId = 1,
                Name = "Calculadora de Viaticos",
                Description = "Herramienta para calcular los viaticos de vuelo",
                PercentageOfCompletion = 0.00M,
                CreatedDate = DateTime.Now.Date,
                EstimatedDate = DateTime.Now.Date.AddDays(5),
                Todos = new List<Todo> (),
                   
            }); 

            modelBuilder.Entity<Project>().HasData(new Project
            {
                ProjectId = 2,
                Name = "Calculadora de Vencimiento",
                Description = "Herramienta para calcular los tiempos limites de vuelo",
                PercentageOfCompletion = 0.00M,
                CreatedDate = DateTime.Now.Date,
                EstimatedDate = DateTime.Now.Date.AddDays(10),
                Todos = new List<Todo>(),
            });
            modelBuilder.Entity<Project>().HasData(new Project
            {
                ProjectId = 3,
                Name = "Todo List",
                Description = "Herramienta para llevar el control de los proyectos",
                PercentageOfCompletion = 0.00M,
                CreatedDate = DateTime.Now.Date,
                EstimatedDate = DateTime.Now.Date.AddDays(30),
                Todos = new List<Todo>(),
            });          
        }
    }
}
