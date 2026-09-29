using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoIA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PrioridadGestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Prioridad",
                table: "Gestiones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.UpdateData(
                table: "Gestiones",
                keyColumn: "Id",
                keyValue: 1,
                column: "Prioridad",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Gestiones",
                keyColumn: "Id",
                keyValue: 2,
                column: "Prioridad",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "Gestiones");
        }
    }
}
