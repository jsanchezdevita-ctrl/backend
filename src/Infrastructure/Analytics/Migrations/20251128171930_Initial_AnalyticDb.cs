using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class Initial_AnalyticDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "accesos_tipo_usuario",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    tipo_usuario = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accesos_tipo_usuario", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accesos_tipo_usuario",
                schema: "public");
        }
    }
}
