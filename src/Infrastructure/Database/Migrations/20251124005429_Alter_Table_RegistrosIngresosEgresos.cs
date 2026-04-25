using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Table_RegistrosIngresosEgresos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_id_punto_entrada",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_id_punto_salida",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.RenameColumn(
                name: "id_punto_salida",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "punto_salida_id");

            migrationBuilder.RenameColumn(
                name: "id_punto_entrada",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "punto_entrada_id");

            migrationBuilder.RenameIndex(
                name: "ix_registros_ingresos_egresos_id_punto_salida",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "ix_registros_ingresos_egresos_punto_salida_id");

            migrationBuilder.RenameIndex(
                name: "ix_registros_ingresos_egresos_id_punto_entrada",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "ix_registros_ingresos_egresos_punto_entrada_id");

            migrationBuilder.AddColumn<Guid>(
                name: "estado_registro_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_registros_ingresos_egresos_estado_registro_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "estado_registro_id");

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_estados_registro_estado_registro",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "estado_registro_id",
                principalSchema: "public",
                principalTable: "estados_registro",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_punto_entrada_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "punto_entrada_id",
                principalSchema: "public",
                principalTable: "puntos_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_punto_salida_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "punto_salida_id",
                principalSchema: "public",
                principalTable: "puntos_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_estados_registro_estado_registro",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_punto_entrada_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_punto_salida_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropIndex(
                name: "ix_registros_ingresos_egresos_estado_registro_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "estado_registro_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.RenameColumn(
                name: "punto_salida_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "id_punto_salida");

            migrationBuilder.RenameColumn(
                name: "punto_entrada_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "id_punto_entrada");

            migrationBuilder.RenameIndex(
                name: "ix_registros_ingresos_egresos_punto_salida_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "ix_registros_ingresos_egresos_id_punto_salida");

            migrationBuilder.RenameIndex(
                name: "ix_registros_ingresos_egresos_punto_entrada_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                newName: "ix_registros_ingresos_egresos_id_punto_entrada");

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_id_punto_entrada",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "id_punto_entrada",
                principalSchema: "public",
                principalTable: "puntos_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_registros_ingresos_egresos_puntos_control_id_punto_salida",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "id_punto_salida",
                principalSchema: "public",
                principalTable: "puntos_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
