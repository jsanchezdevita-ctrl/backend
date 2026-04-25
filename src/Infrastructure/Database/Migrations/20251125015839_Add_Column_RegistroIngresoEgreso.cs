using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_RegistroIngresoEgreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resultado",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.AddColumn<Guid>(
                name: "usuario_id",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usuario_id",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.AddColumn<string>(
                name: "resultado",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
