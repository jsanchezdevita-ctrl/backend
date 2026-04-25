using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_ParametroHistorial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_parametros",
                schema: "public",
                table: "parametros");

            migrationBuilder.AlterColumn<string>(
                name: "valor",
                schema: "public",
                table: "parametros",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                schema: "public",
                table: "parametros",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "actualizado_por",
                schema: "public",
                table: "parametros",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "descripcion",
                schema: "public",
                table: "parametros",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_actualizacion",
                schema: "public",
                table: "parametros",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "version",
                schema: "public",
                table: "parametros",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "pk_parametros",
                schema: "public",
                table: "parametros",
                column: "id");

            migrationBuilder.CreateTable(
                name: "parametros_historial",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    llave = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    valor = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actualizado_por = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parametros_historial", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_parametros_llave",
                schema: "public",
                table: "parametros",
                column: "llave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_parametros_historial_fecha_actualizacion",
                schema: "public",
                table: "parametros_historial",
                column: "fecha_actualizacion");

            migrationBuilder.CreateIndex(
                name: "ix_parametros_historial_llave",
                schema: "public",
                table: "parametros_historial",
                column: "llave");

            migrationBuilder.CreateIndex(
                name: "ix_parametros_historial_llave_version",
                schema: "public",
                table: "parametros_historial",
                columns: new[] { "llave", "version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parametros_historial",
                schema: "public");

            migrationBuilder.DropPrimaryKey(
                name: "pk_parametros",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropIndex(
                name: "ix_parametros_llave",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "actualizado_por",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "descripcion",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "fecha_actualizacion",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "version",
                schema: "public",
                table: "parametros");

            migrationBuilder.AlterColumn<string>(
                name: "valor",
                schema: "public",
                table: "parametros",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AddPrimaryKey(
                name: "pk_parametros",
                schema: "public",
                table: "parametros",
                column: "llave");
        }
    }
}
