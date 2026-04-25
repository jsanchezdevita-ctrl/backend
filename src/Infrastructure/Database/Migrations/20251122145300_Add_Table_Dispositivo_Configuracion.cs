using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_Dispositivo_Configuracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_dispositivos_mac_address",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "mac_address",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "password",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "usuario",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.AddColumn<string>(
                name: "direccion_ip",
                schema: "public",
                table: "dispositivos",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "dispositivo_id",
                schema: "public",
                table: "dispositivos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "estado",
                schema: "public",
                table: "dispositivos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tipo",
                schema: "public",
                table: "dispositivos",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "version_firmware",
                schema: "public",
                table: "dispositivos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "dispositivo_configuraciones",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    dispositivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    frecuencia_sincronizacion_segundos = table.Column<int>(type: "integer", nullable: false),
                    potencia_transmision = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    canal_comunicacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dispositivo_configuraciones", x => x.id);
                    table.ForeignKey(
                        name: "fk_dispositivo_configuraciones_dispositivos_dispositivo_id",
                        column: x => x.dispositivo_id,
                        principalSchema: "public",
                        principalTable: "dispositivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dispositivos_direccion_ip",
                schema: "public",
                table: "dispositivos",
                column: "direccion_ip",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dispositivos_dispositivo_id",
                schema: "public",
                table: "dispositivos",
                column: "dispositivo_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dispositivos_nombre",
                schema: "public",
                table: "dispositivos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dispositivo_configuraciones_dispositivo_id",
                schema: "public",
                table: "dispositivo_configuraciones",
                column: "dispositivo_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dispositivo_configuraciones",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "ix_dispositivos_direccion_ip",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropIndex(
                name: "ix_dispositivos_dispositivo_id",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropIndex(
                name: "ix_dispositivos_nombre",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "direccion_ip",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "dispositivo_id",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "estado",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "tipo",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "version_firmware",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.AddColumn<string>(
                name: "mac_address",
                schema: "public",
                table: "dispositivos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                schema: "public",
                table: "dispositivos",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "usuario",
                schema: "public",
                table: "dispositivos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_dispositivos_mac_address",
                schema: "public",
                table: "dispositivos",
                column: "mac_address",
                unique: true);
        }
    }
}
