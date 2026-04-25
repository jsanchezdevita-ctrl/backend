using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Drop_Columns_RegistronIngresoEgreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_zonas_roles_zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropIndex(
                name: "ix_registros_ingresos_egresos_zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "zona_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
