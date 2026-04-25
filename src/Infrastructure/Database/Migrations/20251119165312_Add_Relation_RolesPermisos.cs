using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Relation_RolesPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_roles_permisos_permiso_id",
                schema: "public",
                table: "roles_permisos",
                column: "permiso_id");

            migrationBuilder.AddForeignKey(
                name: "fk_roles_permisos_permisos_permiso_id",
                schema: "public",
                table: "roles_permisos",
                column: "permiso_id",
                principalSchema: "public",
                principalTable: "permisos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_roles_permisos_roles_rol_id",
                schema: "public",
                table: "roles_permisos",
                column: "rol_id",
                principalSchema: "public",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_roles_permisos_permisos_permiso_id",
                schema: "public",
                table: "roles_permisos");

            migrationBuilder.DropForeignKey(
                name: "fk_roles_permisos_roles_rol_id",
                schema: "public",
                table: "roles_permisos");

            migrationBuilder.DropIndex(
                name: "ix_roles_permisos_permiso_id",
                schema: "public",
                table: "roles_permisos");
        }
    }
}
