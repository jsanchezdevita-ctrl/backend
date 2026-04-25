using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tables_Zonas_ZonasRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zonas_estacionamiento",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "zonas",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zonas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "zonas_roles",
                schema: "public",
                columns: table => new
                {
                    zona_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    capacidad_maxima = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    espacio_utilizado = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zonas_roles", x => new { x.zona_id, x.rol_id });
                });

            migrationBuilder.CreateIndex(
                name: "ix_zonas_nombre",
                schema: "public",
                table: "zonas",
                column: "nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zonas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "zonas_roles",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "zonas_estacionamiento",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    capacidad_maxima = table.Column<int>(type: "integer", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    espacios_utilizados = table.Column<int>(type: "integer", nullable: false),
                    id_punto_asociado = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre_zona = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zonas_estacionamiento", x => x.id);
                    table.ForeignKey(
                        name: "fk_zonas_estacionamiento_puntos_control_id_punto_asociado",
                        column: x => x.id_punto_asociado,
                        principalSchema: "public",
                        principalTable: "puntos_control",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_zonas_estacionamiento_id_punto_asociado",
                schema: "public",
                table: "zonas_estacionamiento",
                column: "id_punto_asociado");
        }
    }
}
