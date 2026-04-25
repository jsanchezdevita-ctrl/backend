using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "dispositivos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    mac_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dispositivos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "parametros",
                schema: "public",
                columns: table => new
                {
                    llave = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    valor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parametros", x => x.llave);
                });

            migrationBuilder.CreateTable(
                name: "puntos_control",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ubicacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    dispositivo_lector_qr = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_puntos_control", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre_rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    apellido = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_ultima_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "registros_ingresos_egresos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    resultado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    id_punto_entrada = table.Column<Guid>(type: "uuid", nullable: false),
                    id_punto_salida = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_registros_ingresos_egresos", x => x.id);
                    table.ForeignKey(
                        name: "fk_registros_ingresos_egresos_puntos_control_id_punto_entrada",
                        column: x => x.id_punto_entrada,
                        principalSchema: "public",
                        principalTable: "puntos_control",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_registros_ingresos_egresos_puntos_control_id_punto_salida",
                        column: x => x.id_punto_salida,
                        principalSchema: "public",
                        principalTable: "puntos_control",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "zonas_estacionamiento",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre_zona = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    capacidad_maxima = table.Column<int>(type: "integer", nullable: false),
                    espacios_utilizados = table.Column<int>(type: "integer", nullable: false),
                    id_punto_asociado = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zonas_estacionamiento", x => x.id);
                    table.ForeignKey(
                        name: "fk_zonas_estacionamiento_puntos_control_id_punto_asociado",
                        column: x => x.id_punto_asociado,
                        principalSchema: "public",
                        principalTable: "puntos_control",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "todo_items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    labels = table.Column<List<string>>(type: "text[]", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_items_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalSchema: "public",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_roles",
                schema: "public",
                columns: table => new
                {
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_asignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios_roles", x => new { x.usuario_id, x.rol_id });
                    table.ForeignKey(
                        name: "fk_usuarios_roles_roles_rol_id",
                        column: x => x.rol_id,
                        principalSchema: "public",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usuarios_roles_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalSchema: "public",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dispositivos_mac_address",
                schema: "public",
                table: "dispositivos",
                column: "mac_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_registros_ingresos_egresos_id_punto_entrada",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "id_punto_entrada");

            migrationBuilder.CreateIndex(
                name: "ix_registros_ingresos_egresos_id_punto_salida",
                schema: "public",
                table: "registros_ingresos_egresos",
                column: "id_punto_salida");

            migrationBuilder.CreateIndex(
                name: "ix_roles_nombre_rol",
                schema: "public",
                table: "roles",
                column: "nombre_rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_todo_items_usuario_id",
                schema: "public",
                table: "todo_items",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_email",
                schema: "public",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_roles_rol_id",
                schema: "public",
                table: "usuarios_roles",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "ix_zonas_estacionamiento_id_punto_asociado",
                schema: "public",
                table: "zonas_estacionamiento",
                column: "id_punto_asociado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dispositivos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "parametros",
                schema: "public");

            migrationBuilder.DropTable(
                name: "registros_ingresos_egresos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "todo_items",
                schema: "public");

            migrationBuilder.DropTable(
                name: "usuarios_roles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "zonas_estacionamiento",
                schema: "public");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "public");

            migrationBuilder.DropTable(
                name: "puntos_control",
                schema: "public");
        }
    }
}
