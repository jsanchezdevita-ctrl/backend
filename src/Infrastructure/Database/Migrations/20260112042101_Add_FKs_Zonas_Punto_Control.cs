using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_FKs_Zonas_Punto_Control : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_zonas_roles_rol_id",
                schema: "public",
                table: "zonas_roles",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "ix_zonas_puntos_control_punto_control_id",
                schema: "public",
                table: "zonas_puntos_control",
                column: "punto_control_id");

            migrationBuilder.AddForeignKey(
                name: "fk_zonas_puntos_control_puntos_control_punto_control_id",
                schema: "public",
                table: "zonas_puntos_control",
                column: "punto_control_id",
                principalSchema: "public",
                principalTable: "puntos_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_zonas_puntos_control_zonas_zona_id",
                schema: "public",
                table: "zonas_puntos_control",
                column: "zona_id",
                principalSchema: "public",
                principalTable: "zonas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_zonas_roles_roles_rol_id",
                schema: "public",
                table: "zonas_roles",
                column: "rol_id",
                principalSchema: "public",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_zonas_roles_zonas_zona_id",
                schema: "public",
                table: "zonas_roles",
                column: "zona_id",
                principalSchema: "public",
                principalTable: "zonas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_zonas_puntos_control_puntos_control_punto_control_id",
                schema: "public",
                table: "zonas_puntos_control");

            migrationBuilder.DropForeignKey(
                name: "fk_zonas_puntos_control_zonas_zona_id",
                schema: "public",
                table: "zonas_puntos_control");

            migrationBuilder.DropForeignKey(
                name: "fk_zonas_roles_roles_rol_id",
                schema: "public",
                table: "zonas_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_zonas_roles_zonas_zona_id",
                schema: "public",
                table: "zonas_roles");

            migrationBuilder.DropIndex(
                name: "ix_zonas_roles_rol_id",
                schema: "public",
                table: "zonas_roles");

            migrationBuilder.DropIndex(
                name: "ix_zonas_puntos_control_punto_control_id",
                schema: "public",
                table: "zonas_puntos_control");
        }
    }
}
