using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Columns_RegistronIngresoEgreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "rol_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "zona_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_registros_ingresos_egresos_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "ix_registros_ingresos_egresos_zona_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "zona_id");

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_roles_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "rol_id",
                principalSchema: "public",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_zonas_zona_id",
                schema: "public",
                table: "registros_ingresos_egresos",
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
                name: "fk_registros_ingresos_egresos_roles_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_zonas_zona_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropIndex(
                name: "ix_registros_ingresos_egresos_rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropIndex(
                name: "ix_registros_ingresos_egresos_zona_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "rol_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "zona_id",
                schema: "public",
                table: "registros_ingresos_egresos");
        }
    }
}
