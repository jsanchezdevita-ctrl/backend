using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Table_QrTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_qr_tokens_zonas_roles_zona_rol_id",
                schema: "public",
                table: "qr_tokens");

            migrationBuilder.RenameColumn(
                name: "zona_rol_id",
                schema: "public",
                table: "qr_tokens",
                newName: "zona_id");

            migrationBuilder.RenameIndex(
                name: "ix_qr_tokens_zona_rol_id",
                schema: "public",
                table: "qr_tokens",
                newName: "ix_qr_tokens_zona_id");

            migrationBuilder.AddForeignKey(
                name: "fk_qr_tokens_zonas_zona_id",
                schema: "public",
                table: "qr_tokens",
                column: "zona_id",
                principalSchema: "public",
                principalTable: "zonas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_qr_tokens_zonas_zona_id",
                schema: "public",
                table: "qr_tokens");

            migrationBuilder.RenameColumn(
                name: "zona_id",
                schema: "public",
                table: "qr_tokens",
                newName: "zona_rol_id");

            migrationBuilder.RenameIndex(
                name: "ix_qr_tokens_zona_id",
                schema: "public",
                table: "qr_tokens",
                newName: "ix_qr_tokens_zona_rol_id");

            migrationBuilder.AddForeignKey(
                name: "fk_qr_tokens_zonas_roles_zona_rol_id",
                schema: "public",
                table: "qr_tokens",
                column: "zona_rol_id",
                principalSchema: "public",
                principalTable: "zonas_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
