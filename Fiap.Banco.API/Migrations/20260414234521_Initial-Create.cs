using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Banco.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgenciaBanco",
                columns: table => new
                {
                    idAgencia = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nmEndereco = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cep = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenciaBanco", x => x.idAgencia);
                });

            migrationBuilder.CreateTable(
                name: "Bancos",
                columns: table => new
                {
                    idBanco = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nomeBanco = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dtCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CEP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.idBanco);
                });

            migrationBuilder.CreateTable(
                name: "ClientesBanco",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nmCliente = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesBanco", x => x.idCliente);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgenciaBanco");

            migrationBuilder.DropTable(
                name: "Bancos");

            migrationBuilder.DropTable(
                name: "ClientesBanco");
        }
    }
}
