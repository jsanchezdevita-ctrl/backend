using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Table_Puntos_Control : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "descripcion",
                schema: "public",
                table: "puntos_control",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre",
                schema: "public",
                table: "puntos_control",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_puntos_control_nombre",
                schema: "public",
                table: "puntos_control",
                column: "nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_puntos_control_nombre",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropColumn(
                name: "descripcion",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropColumn(
                name: "nombre",
                schema: "public",
                table: "puntos_control");
        }
    }
}
