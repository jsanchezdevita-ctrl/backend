using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Drop_Column_Dispositivos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estado",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "tipo",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "version_firmware",
                schema: "public",
                table: "dispositivos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "estado",
                schema: "public",
                table: "dispositivos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tipo",
                schema: "public",
                table: "dispositivos",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "version_firmware",
                schema: "public",
                table: "dispositivos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
