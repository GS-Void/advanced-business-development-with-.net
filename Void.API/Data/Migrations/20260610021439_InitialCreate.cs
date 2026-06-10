using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Void.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOG_AUDITORIA_SESSAO",
                columns: table => new
                {
                    DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ACAO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    PACIENTE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DATA_SESSAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STATUS_ANTIGO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PROTOCOLO_ESPACIAL",
                columns: table => new
                {
                    ID_PROTOCOLO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_PROTOCOLO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    LIMITE_FADIGA_MAXIMA = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROTOCOLO_ESPACIAL", x => x.ID_PROTOCOLO);
                });

            migrationBuilder.CreateTable(
                name: "SENSOR_WEARABLE",
                columns: table => new
                {
                    ID_SENSOR = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MAC_ADDRESS = table.Column<string>(type: "NVARCHAR2(17)", maxLength: 17, nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SENSOR_WEARABLE", x => x.ID_SENSOR);
                });

            migrationBuilder.CreateTable(
                name: "TB_VOID_USUARIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TIPO_USUARIO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VOID_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TELEMETRIA_RAW_JSON",
                columns: table => new
                {
                    ID_LOG = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PACIENTE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_SESSAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DADOS_JSON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TELEMETRIA_RAW_JSON", x => x.ID_LOG);
                });

            migrationBuilder.CreateTable(
                name: "TB_VOID_FISIOTERAPEUTA",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    REGISTRO_PROFISSIONAL = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VOID_FISIOTERAPEUTA", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_VOID_FISIOTERAPEUTA_TB_VOID_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_VOID_USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_VOID_PACIENTE",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LIMITE_ESFORCO_CRITICO = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VOID_PACIENTE", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_VOID_PACIENTE_TB_VOID_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_VOID_USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_VOID_SESSAO_REABILITACAO",
                columns: table => new
                {
                    PACIENTE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_SESSAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DESGASTE_ACUMULADO = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ALERTA_FADIGA_CRITICA = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_FISIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_PROTOCOLO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS_SESSAO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VOID_SESSAO_REABILITACAO", x => new { x.PACIENTE_ID, x.DATA_SESSAO });
                    table.ForeignKey(
                        name: "FK_TB_VOID_SESSAO_REABILITACAO_PROTOCOLO_ESPACIAL_ID_PROTOCOLO",
                        column: x => x.ID_PROTOCOLO,
                        principalTable: "PROTOCOLO_ESPACIAL",
                        principalColumn: "ID_PROTOCOLO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_VOID_SESSAO_REABILITACAO_TB_VOID_FISIOTERAPEUTA_ID_FISIO",
                        column: x => x.ID_FISIO,
                        principalTable: "TB_VOID_FISIOTERAPEUTA",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_VOID_SESSAO_REABILITACAO_TB_VOID_PACIENTE_PACIENTE_ID",
                        column: x => x.PACIENTE_ID,
                        principalTable: "TB_VOID_PACIENTE",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ALERTA_CRITICO",
                columns: table => new
                {
                    ID_ALERTA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PACIENTE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_SESSAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TIMESTAMP_ALERTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NIVEL_ATINGIDO = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTA_CRITICO", x => x.ID_ALERTA);
                    table.ForeignKey(
                        name: "FK_ALERTA_CRITICO_TB_VOID_SESSAO_REABILITACAO_PACIENTE_ID_DATA_SESSAO",
                        columns: x => new { x.PACIENTE_ID, x.DATA_SESSAO },
                        principalTable: "TB_VOID_SESSAO_REABILITACAO",
                        principalColumns: new[] { "PACIENTE_ID", "DATA_SESSAO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LEITURA_FADIGA",
                columns: table => new
                {
                    PACIENTE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_SESSAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    SEGUNDO_LEITURA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_SENSOR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PERCENTUAL_DESGASTE = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEITURA_FADIGA", x => new { x.PACIENTE_ID, x.DATA_SESSAO, x.SEGUNDO_LEITURA });
                    table.ForeignKey(
                        name: "FK_LEITURA_FADIGA_SENSOR_WEARABLE_ID_SENSOR",
                        column: x => x.ID_SENSOR,
                        principalTable: "SENSOR_WEARABLE",
                        principalColumn: "ID_SENSOR",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LEITURA_FADIGA_TB_VOID_SESSAO_REABILITACAO_PACIENTE_ID_DATA_SESSAO",
                        columns: x => new { x.PACIENTE_ID, x.DATA_SESSAO },
                        principalTable: "TB_VOID_SESSAO_REABILITACAO",
                        principalColumns: new[] { "PACIENTE_ID", "DATA_SESSAO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERTA_CRITICO_PACIENTE_ID_DATA_SESSAO",
                table: "ALERTA_CRITICO",
                columns: new[] { "PACIENTE_ID", "DATA_SESSAO" });

            migrationBuilder.CreateIndex(
                name: "IX_LEITURA_FADIGA_ID_SENSOR",
                table: "LEITURA_FADIGA",
                column: "ID_SENSOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_VOID_SESSAO_REABILITACAO_ID_FISIO",
                table: "TB_VOID_SESSAO_REABILITACAO",
                column: "ID_FISIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_VOID_SESSAO_REABILITACAO_ID_PROTOCOLO",
                table: "TB_VOID_SESSAO_REABILITACAO",
                column: "ID_PROTOCOLO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALERTA_CRITICO");

            migrationBuilder.DropTable(
                name: "LEITURA_FADIGA");

            migrationBuilder.DropTable(
                name: "LOG_AUDITORIA_SESSAO");

            migrationBuilder.DropTable(
                name: "TELEMETRIA_RAW_JSON");

            migrationBuilder.DropTable(
                name: "SENSOR_WEARABLE");

            migrationBuilder.DropTable(
                name: "TB_VOID_SESSAO_REABILITACAO");

            migrationBuilder.DropTable(
                name: "PROTOCOLO_ESPACIAL");

            migrationBuilder.DropTable(
                name: "TB_VOID_FISIOTERAPEUTA");

            migrationBuilder.DropTable(
                name: "TB_VOID_PACIENTE");

            migrationBuilder.DropTable(
                name: "TB_VOID_USUARIO");
        }
    }
}
