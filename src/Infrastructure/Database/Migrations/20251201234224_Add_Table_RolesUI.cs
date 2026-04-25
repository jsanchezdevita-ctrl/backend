using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_RolesUI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles_ui",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text_color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false, defaultValue: "#000000"),
                    background_color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false, defaultValue: "#FFFFFF"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles_ui", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_roles_ui_rol_id",
                schema: "public",
                table: "roles_ui",
                column: "rol_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roles_ui",
                schema: "public");
        }
    }
}
