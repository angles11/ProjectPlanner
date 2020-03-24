using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectPlanner.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EstimatedDate = table.Column<DateTime>(nullable: false),
                    PercentageOfCompletion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    TodoId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EstimatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_Todos_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "CreatedDate", "Description", "EstimatedDate", "Name", "PercentageOfCompletion" },
                values: new object[] { 1, new DateTime(2020, 3, 24, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para calcular los viaticos de vuelo", new DateTime(2020, 3, 29, 0, 0, 0, 0, DateTimeKind.Local), "Calculadora de Viaticos", 0.00m });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "CreatedDate", "Description", "EstimatedDate", "Name", "PercentageOfCompletion" },
                values: new object[] { 2, new DateTime(2020, 3, 24, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para calcular los tiempos limites de vuelo", new DateTime(2020, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), "Calculadora de Vencimiento", 0.00m });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "CreatedDate", "Description", "EstimatedDate", "Name", "PercentageOfCompletion" },
                values: new object[] { 3, new DateTime(2020, 3, 24, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para llevar el control de los proyectos", new DateTime(2020, 4, 23, 0, 0, 0, 0, DateTimeKind.Local), "Todo List", 0.00m });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_ProjectId",
                table: "Todos",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
