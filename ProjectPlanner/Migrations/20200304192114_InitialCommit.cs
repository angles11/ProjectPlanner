using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectPlanner.Migrations
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EstimatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    TodoId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MyProperty = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EstimatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_Todos_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "EstimatedDate", "Name", "Status" },
                values: new object[] { 1, new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para calcular los viaticos de vuelo", new DateTime(2020, 3, 9, 16, 21, 14, 385, DateTimeKind.Local).AddTicks(6517), "Calculadora de Viaticos", "Pending" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "EstimatedDate", "Name", "Status" },
                values: new object[] { 2, new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para calcular los tiempos limites de vuelo", new DateTime(2020, 3, 14, 16, 21, 14, 387, DateTimeKind.Local).AddTicks(2899), "Calculadora de Vencimiento", "In Progress" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "EstimatedDate", "Name", "Status" },
                values: new object[] { 3, new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para llevar el control de los proyectos", new DateTime(2020, 4, 3, 16, 21, 14, 387, DateTimeKind.Local).AddTicks(3043), "Todo List", "Completed" });

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
