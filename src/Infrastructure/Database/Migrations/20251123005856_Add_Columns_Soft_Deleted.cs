using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Columns_Soft_Deleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "zonas_estacionamiento",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "zonas_estacionamiento",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "zonas_estacionamiento",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "usuarios_roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "usuarios_roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "usuarios_roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "usuarios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "roles_permisos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "roles_permisos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "roles_permisos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "registros_ingresos_egresos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "refresh_tokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "refresh_tokens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "puntos_control",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "puntos_control",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "puntos_control",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "permisos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "permisos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "permisos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "parametros",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "parametros",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "parametros",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "dispositivos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "dispositivos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "dispositivos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "dispositivo_configuraciones",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                schema: "public",
                table: "dispositivo_configuraciones",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                schema: "public",
                table: "dispositivo_configuraciones",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "zonas_estacionamiento");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "zonas_estacionamiento");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "zonas_estacionamiento");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "usuarios_roles");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "roles_permisos");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "roles_permisos");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "roles_permisos");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "registros_ingresos_egresos");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "puntos_control");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "permisos");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "permisos");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "permisos");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "dispositivos");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "dispositivo_configuraciones");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "public",
                table: "dispositivo_configuraciones");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                schema: "public",
                table: "dispositivo_configuraciones");
        }
    }
}
