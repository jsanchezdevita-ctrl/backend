using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Columm_PuntoControlId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispositivos_puntos_control_punto_control_id",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.AlterColumn<Guid>(
                name: "punto_control_id",
                schema: "public",
                table: "dispositivos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_dispositivos_puntos_control_punto_control_id",
                schema: "public",
                table: "dispositivos",
                column: "punto_control_id",
                principalSchema: "public",
                principalTable: "puntos_control",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispositivos_puntos_control_punto_control_id",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.AlterColumn<Guid>(
                name: "punto_control_id",
                schema: "public",
                table: "dispositivos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_dispositivos_puntos_control_punto_control_id",
                schema: "public",
                table: "dispositivos",
                column: "punto_control_id",
                principalSchema: "public",
                principalTable: "puntos_control",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
