using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_Accesos_Incidencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accesos_incidencias",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    punto_control_id = table.Column<Guid>(type: "uuid", nullable: false),
                    incidencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accesos_incidencias", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accesos_incidencias_punto_control_id_incidencia_fecha",
                schema: "public",
                table: "accesos_incidencias",
                columns: new[] { "punto_control_id", "incidencia", "fecha" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accesos_incidencias",
                schema: "public");
        }
    }
}
