using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_table_ZonasPuntosControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "zonas_puntos_control",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    zona_id = table.Column<Guid>(type: "uuid", nullable: false),
                    punto_control_id = table.Column<Guid>(type: "uuid", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zonas_puntos_control", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_zonas_puntos_control_zona_id_punto_control_id",
                schema: "public",
                table: "zonas_puntos_control",
                columns: new[] { "zona_id", "punto_control_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zonas_puntos_control",
                schema: "public");
        }
    }
}
