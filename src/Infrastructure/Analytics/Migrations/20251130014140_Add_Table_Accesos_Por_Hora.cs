using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_Accesos_Por_Hora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accesos_por_hora",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    hora = table.Column<int>(type: "integer", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accesos_por_hora", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accesos_por_hora_fecha_hora",
                schema: "public",
                table: "accesos_por_hora",
                columns: new[] { "fecha", "hora" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accesos_por_hora",
                schema: "public");
        }
    }
}
