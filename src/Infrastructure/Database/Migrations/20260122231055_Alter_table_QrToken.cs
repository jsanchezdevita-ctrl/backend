using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alter_table_QrToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "rol_id",
                schema: "public",
                table: "qr_tokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_qr_tokens_rol_id",
                schema: "public",
                table: "qr_tokens",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "ix_qr_tokens_usuario_id",
                schema: "public",
                table: "qr_tokens",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "fk_qr_tokens_roles_rol_id",
                schema: "public",
                table: "qr_tokens",
                column: "rol_id",
                principalSchema: "public",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_qr_tokens_usuarios_usuario_id",
                schema: "public",
                table: "qr_tokens",
                column: "usuario_id",
                principalSchema: "public",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_qr_tokens_roles_rol_id",
                schema: "public",
                table: "qr_tokens");

            migrationBuilder.DropForeignKey(
                name: "fk_qr_tokens_usuarios_usuario_id",
                schema: "public",
                table: "qr_tokens");

            migrationBuilder.DropIndex(
                name: "ix_qr_tokens_rol_id",
                schema: "public",
                table: "qr_tokens");

            migrationBuilder.DropIndex(
                name: "ix_qr_tokens_usuario_id",
                schema: "public",
                table: "qr_tokens");

            migrationBuilder.DropColumn(
                name: "rol_id",
                schema: "public",
                table: "qr_tokens");
        }
    }
}
