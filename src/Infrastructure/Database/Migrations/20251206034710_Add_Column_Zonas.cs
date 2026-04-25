using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_Zonas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_zonas_roles",
                schema: "public",
                table: "zonas_roles");

            migrationBuilder.AddColumn<Guid>(
                name: "zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_zonas_roles",
                schema: "public",
                table: "zonas_roles",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_zonas_roles_zona_id_rol_id",
                schema: "public",
                table: "zonas_roles",
                columns: new[] { "zona_id", "rol_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_registros_ingresos_egresos_zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "zona_rol_id");

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_zonas_roles_zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "zona_rol_id",
                principalSchema: "public",
                principalTable: "zonas_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_zonas_roles_zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_zonas_roles",
                schema: "public",
                table: "zonas_roles");

            migrationBuilder.DropIndex(
                name: "ix_zonas_roles_zona_id_rol_id",
                schema: "public",
                table: "zonas_roles");

            migrationBuilder.DropIndex(
                name: "ix_registros_ingresos_egresos_zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.AddPrimaryKey(
                name: "pk_zonas_roles",
                schema: "public",
                table: "zonas_roles",
                columns: new[] { "zona_id", "rol_id" });
        }
    }
}
