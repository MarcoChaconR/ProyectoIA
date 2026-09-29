using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoIA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CatalogosAdministrables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Usuarios",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "TiposSolicitud",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Dependencias",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CecoId",
                table: "Dependencias",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Cecos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Cecos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Activo", "Codigo", "Nombre" },
                values: new object[] { true, "5550", "Desarrollo de software" });

            migrationBuilder.UpdateData(
                table: "Cecos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Activo", "Codigo" },
                values: new object[] { true, "5551" });

            migrationBuilder.UpdateData(
                table: "Dependencias",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Activa", "CecoId" },
                values: new object[] { true, 1 });

            migrationBuilder.UpdateData(
                table: "Dependencias",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Activa", "CecoId" },
                values: new object[] { true, 2 });

            migrationBuilder.UpdateData(
                table: "TiposSolicitud",
                keyColumn: "Id",
                keyValue: 1,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "TiposSolicitud",
                keyColumn: "Id",
                keyValue: 2,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "TiposSolicitud",
                keyColumn: "Id",
                keyValue: 3,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "Activo",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dependencias_CecoId",
                table: "Dependencias",
                column: "CecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cecos_Codigo",
                table: "Cecos",
                column: "Codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dependencias_Cecos_CecoId",
                table: "Dependencias",
                column: "CecoId",
                principalTable: "Cecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dependencias_Cecos_CecoId",
                table: "Dependencias");

            migrationBuilder.DropIndex(
                name: "IX_Dependencias_CecoId",
                table: "Dependencias");

            migrationBuilder.DropIndex(
                name: "IX_Cecos_Codigo",
                table: "Cecos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "TiposSolicitud");

            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "CecoId",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Cecos");

            migrationBuilder.UpdateData(
                table: "Cecos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Codigo", "Nombre" },
                values: new object[] { "CECO-001", "Centro de Costo Principal" });

            migrationBuilder.UpdateData(
                table: "Cecos",
                keyColumn: "Id",
                keyValue: 2,
                column: "Codigo",
                value: "CECO-002");
        }
    }
}
