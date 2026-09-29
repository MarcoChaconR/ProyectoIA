using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoIA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cecos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cecos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dependencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposSolicitud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposSolicitud", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Correo = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Rol = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gestiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CecoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DependenciaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoSolicitudId = table.Column<int>(type: "INTEGER", nullable: false),
                    Objetivo = table.Column<string>(type: "TEXT", nullable: false),
                    Detalle = table.Column<string>(type: "TEXT", nullable: false),
                    ReferenciaIngreso = table.Column<string>(type: "TEXT", nullable: true),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaCambioEstado = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SolicitanteId = table.Column<int>(type: "INTEGER", nullable: false),
                    TecnicoAsignadoId = table.Column<int>(type: "INTEGER", nullable: true),
                    FechaAsignacion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gestiones_Cecos_CecoId",
                        column: x => x.CecoId,
                        principalTable: "Cecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gestiones_Dependencias_DependenciaId",
                        column: x => x.DependenciaId,
                        principalTable: "Dependencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gestiones_TiposSolicitud_TipoSolicitudId",
                        column: x => x.TipoSolicitudId,
                        principalTable: "TiposSolicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gestiones_Usuarios_SolicitanteId",
                        column: x => x.SolicitanteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gestiones_Usuarios_TecnicoAsignadoId",
                        column: x => x.TecnicoAsignadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bitacora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Accion = table.Column<string>(type: "TEXT", nullable: false),
                    ValorAnterior = table.Column<string>(type: "TEXT", nullable: true),
                    ValorNuevo = table.Column<string>(type: "TEXT", nullable: true),
                    AutorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bitacora_Gestiones_GestionId",
                        column: x => x.GestionId,
                        principalTable: "Gestiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bitacora_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Texto = table.Column<string>(type: "TEXT", nullable: false),
                    EsPublica = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notas_Gestiones_GestionId",
                        column: x => x.GestionId,
                        principalTable: "Gestiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notas_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cecos",
                columns: new[] { "Id", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, "CECO-001", "Centro de Costo Principal" },
                    { 2, "CECO-002", "Centro de Costo Secundario" }
                });

            migrationBuilder.InsertData(
                table: "Dependencias",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Operaciones" },
                    { 2, "Finanzas" }
                });

            migrationBuilder.InsertData(
                table: "TiposSolicitud",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "RPA" },
                    { 2, "BPM" },
                    { 3, "Power Platform" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Correo", "Nombre", "PasswordHash", "Rol" },
                values: new object[,]
                {
                    { 1, "admin@proyectoia.com", "Administrador", "3EB3FE66B31E3B4D10FA70B5CAD49C7112294AF6AE4E476A1C405155D45AA121", 3 },
                    { 2, "tecnico@proyectoia.com", "Técnico", "91C8C18A270E60459C62B1491F4314923E660E8B2030A33BD7F135BBDE990C30", 2 },
                    { 3, "cliente@proyectoia.com", "Cliente", "519F24A823B10612516C33F438F12967A179AB77D6E7D2AF68F3B9F2631345A7", 1 }
                });

            migrationBuilder.InsertData(
                table: "Gestiones",
                columns: new[] { "Id", "CecoId", "DependenciaId", "Detalle", "Estado", "Fecha", "FechaAsignacion", "FechaCambioEstado", "Objetivo", "ReferenciaIngreso", "SolicitanteId", "TecnicoAsignadoId", "TipoSolicitudId" },
                values: new object[,]
                {
                    { 1, 1, 1, "Automatizar el proceso mensual de conciliación para reducir el trabajo manual.", 1, new DateTime(2026, 9, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 9, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Automatizar conciliación bancaria", "MEMO-001", 3, null, 1 },
                    { 2, 2, 2, "Digitalizar el flujo de aprobación de órdenes de compra.", 2, new DateTime(2026, 9, 2, 10, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 10, 30, 0, 0, DateTimeKind.Unspecified), "Flujo de aprobación de compras", "MEMO-002", 3, 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_AutorId",
                table: "Bitacora",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_GestionId",
                table: "Bitacora",
                column: "GestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_CecoId",
                table: "Gestiones",
                column: "CecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_DependenciaId",
                table: "Gestiones",
                column: "DependenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_SolicitanteId",
                table: "Gestiones",
                column: "SolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_TecnicoAsignadoId",
                table: "Gestiones",
                column: "TecnicoAsignadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_TipoSolicitudId",
                table: "Gestiones",
                column: "TipoSolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AutorId",
                table: "Notas",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_GestionId",
                table: "Notas",
                column: "GestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bitacora");

            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "Gestiones");

            migrationBuilder.DropTable(
                name: "Cecos");

            migrationBuilder.DropTable(
                name: "Dependencias");

            migrationBuilder.DropTable(
                name: "TiposSolicitud");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
