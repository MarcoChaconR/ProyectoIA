using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoIA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "pbkdf2-sha256$210000$cHJveWVjdG9pYS1zZWVkLWFkbWluLTIwMjY=$CU1bAqYrRU5XBNjPU5HIn9xwudtVyCE84HEzRZWOm6Y=");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "pbkdf2-sha256$210000$cHJveWVjdG9pYS1zZWVkLXRlY25pY28tMjAyNg==$d4Pe+2qkPs1VGvS90MVxG/VLxAKKfjJT4MCKFX+0iSc=");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "pbkdf2-sha256$210000$cHJveWVjdG9pYS1zZWVkLWNsaWVudGUtMjAyNg==$h9IAWGSVDxRotF/qFgyhtvg0+b6mKtqS2cIpvDxLWAY=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "3EB3FE66B31E3B4D10FA70B5CAD49C7112294AF6AE4E476A1C405155D45AA121");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "91C8C18A270E60459C62B1491F4314923E660E8B2030A33BD7F135BBDE990C30");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "519F24A823B10612516C33F438F12967A179AB77D6E7D2AF68F3B9F2631345A7");
        }
    }
}
