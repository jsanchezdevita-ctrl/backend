using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_QrTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "qr_tokens",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_expiracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_uso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dispositivo_id = table.Column<Guid>(type: "uuid", nullable: true),
                    punto_control_id = table.Column<Guid>(type: "uuid", nullable: true),
                    zona_rol_id = table.Column<Guid>(type: "uuid", nullable: true),
                    registro_ingreso_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_qr_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_qr_tokens_dispositivos_dispositivo_id",
                        column: x => x.dispositivo_id,
                        principalSchema: "public",
                        principalTable: "dispositivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_qr_tokens_puntos_control_punto_control_id",
                        column: x => x.punto_control_id,
                        principalSchema: "public",
                        principalTable: "puntos_control",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_qr_tokens_registros_ingresos_egresos_registro_ingreso_id",
                        column: x => x.registro_ingreso_id,
                        principalSchema: "public",
                        principalTable: "registros_ingresos_egresos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_qr_tokens_zonas_roles_zona_rol_id",
                        column: x => x.zona_rol_id,
                        principalSchema: "public",
                        principalTable: "zonas_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_qr_tokens_dispositivo_id",
                schema: "public",
                table: "qr_tokens",
                column: "dispositivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_qr_tokens_punto_control_id",
                schema: "public",
                table: "qr_tokens",
                column: "punto_control_id");

            migrationBuilder.CreateIndex(
                name: "ix_qr_tokens_registro_ingreso_id",
                schema: "public",
                table: "qr_tokens",
                column: "registro_ingreso_id");

            migrationBuilder.CreateIndex(
                name: "ix_qr_tokens_token",
                schema: "public",
                table: "qr_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_qr_tokens_zona_rol_id",
                schema: "public",
                table: "qr_tokens",
                column: "zona_rol_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qr_tokens",
                schema: "public");
        }
    }
}
