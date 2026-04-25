using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Columns_Table_Puntos_Control : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "rol_id",
                schema: "public",
                table: "puntos_control",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_puntos_control_rol_id",
                schema: "public",
                table: "puntos_control",
                column: "rol_id");

            migrationBuilder.AddForeignKey(
                name: "fk_puntos_control_roles_rol_id",
                schema: "public",
                table: "puntos_control",
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
                name: "fk_puntos_control_roles_rol_id",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropIndex(
                name: "ix_puntos_control_rol_id",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropColumn(
                name: "rol_id",
                schema: "public",
                table: "puntos_control");
        }
    }
}
