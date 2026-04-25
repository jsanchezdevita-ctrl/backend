using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Columm_PuntoControlId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "punto_control_id",
                schema: "public",
                table: "dispositivos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_dispositivos_punto_control_id",
                schema: "public",
                table: "dispositivos",
                column: "punto_control_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispositivos_puntos_control_punto_control_id",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropIndex(
                name: "ix_dispositivos_punto_control_id",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "punto_control_id",
                schema: "public",
                table: "dispositivos");
        }
    }
}
