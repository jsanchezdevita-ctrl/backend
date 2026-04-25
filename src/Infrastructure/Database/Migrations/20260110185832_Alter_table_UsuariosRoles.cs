using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alter_table_UsuariosRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_usuarios_roles",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.DropIndex(
                name: "ix_usuarios_roles_usuario_id",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.AddPrimaryKey(
                name: "pk_usuarios_roles",
                schema: "public",
                table: "usuarios_roles",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_roles_usuario_id",
                schema: "public",
                table: "usuarios_roles",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_roles_usuario_id_rol_id",
                schema: "public",
                table: "usuarios_roles",
                columns: new[] { "usuario_id", "rol_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_usuarios_roles",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.DropIndex(
                name: "ix_usuarios_roles_usuario_id",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.DropIndex(
                name: "ix_usuarios_roles_usuario_id_rol_id",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.AddPrimaryKey(
                name: "pk_usuarios_roles",
                schema: "public",
                table: "usuarios_roles",
                columns: new[] { "usuario_id", "rol_id" });

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_roles_usuario_id",
                schema: "public",
                table: "usuarios_roles",
                column: "usuario_id",
                unique: true);
        }
    }
}
