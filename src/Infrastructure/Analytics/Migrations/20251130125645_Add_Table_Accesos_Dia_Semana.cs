using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_Accesos_Dia_Semana : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accesos_dia_semana",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    dia_semana_completo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    dia_semana_corto = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accesos_dia_semana", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accesos_dia_semana_fecha_dia_semana_completo",
                schema: "public",
                table: "accesos_dia_semana",
                columns: new[] { "fecha", "dia_semana_completo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accesos_dia_semana",
                schema: "public");
        }
    }
}
