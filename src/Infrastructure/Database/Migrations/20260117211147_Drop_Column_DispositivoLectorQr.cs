using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Drop_Column_DispositivoLectorQr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dispositivo_lector_qr",
                schema: "public",
                table: "puntos_control");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dispositivo_lector_qr",
                schema: "public",
                table: "puntos_control",
                type: "jsonb",
                nullable: true);
        }
    }
}
