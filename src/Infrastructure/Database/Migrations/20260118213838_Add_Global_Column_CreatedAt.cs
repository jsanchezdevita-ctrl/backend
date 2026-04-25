using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Global_Column_CreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "zonas_roles",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "zonas_puntos_control",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "zonas",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "usuarios_roles",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "usuarios",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "roles_ui",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "roles_permisos",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "roles",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "refresh_tokens",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "qr_tokens",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "puntos_control",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "permisos",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "parametros_historial",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "parametros",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "parametro_schemas",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "estados_registro",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "dispositivos",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "public",
                table: "dispositivo_configuraciones",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "zonas_roles");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "zonas_puntos_control");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "zonas");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "roles_ui");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "roles_permisos");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "qr_tokens");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "permisos");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "parametros_historial");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "parametro_schemas");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "estados_registro");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "public",
                table: "dispositivo_configuraciones");
        }
    }
}
