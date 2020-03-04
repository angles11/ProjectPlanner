using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectPlanner.Migrations
{
    public partial class modelBuilderCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Projects");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Todos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "EstimatedDate", "Name", "Status" },
                values: new object[] { 1, new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para calcular los viaticos de vuelo", new DateTime(2020, 3, 9, 16, 15, 20, 0, DateTimeKind.Local).AddTicks(8452), "Calculadora de Viaticos", "Pending" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "EstimatedDate", "Name", "Status" },
                values: new object[] { 2, new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para calcular los tiempos limites de vuelo", new DateTime(2020, 3, 14, 16, 15, 20, 2, DateTimeKind.Local).AddTicks(6269), "Calculadora de Vencimiento", "In Progress" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "EstimatedDate", "Name", "Status" },
                values: new object[] { 3, new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Local), "Herramienta para llevar el control de los proyectos", new DateTime(2020, 4, 3, 16, 15, 20, 2, DateTimeKind.Local).AddTicks(6447), "Todo List", "Completed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Projects");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
