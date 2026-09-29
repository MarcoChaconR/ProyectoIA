using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoIA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GestionConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Gestiones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Gestiones",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Gestiones",
                keyColumn: "Id",
                keyValue: 2,
                column: "Version",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Gestiones");
        }
    }
}
