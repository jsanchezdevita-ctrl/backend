using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tables_Permision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permisos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permisos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles_permisos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    permiso_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles_permisos", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_permisos_nombre",
                schema: "public",
                table: "permisos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_permisos_rol_id_permiso_id",
                schema: "public",
                table: "roles_permisos",
                columns: new[] { "rol_id", "permiso_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permisos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "roles_permisos",
                schema: "public");
        }
    }
}
