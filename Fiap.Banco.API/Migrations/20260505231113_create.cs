using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Banco.API.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nmCliente",
                table: "ClientesBanco",
                type: "NVARCHAR2(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AddColumn<string>(
                name: "CNPJ",
                table: "ClientesBanco",
                type: "NVARCHAR2(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "ClientesBanco",
                type: "NVARCHAR2(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "ClientesBanco",
                type: "TIMESTAMP(7)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "ClientesBanco",
                type: "NVARCHAR2(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoCliente",
                table: "ClientesBanco",
                type: "NVARCHAR2(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "idAgencia",
                table: "ClientesBanco",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "nomeBanco",
                table: "Bancos",
                type: "NVARCHAR2(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Bancos",
                type: "NVARCHAR2(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "nmEndereco",
                table: "AgenciaBanco",
                type: "NVARCHAR2(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "cep",
                table: "AgenciaBanco",
                type: "NVARCHAR2(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    idProduto = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nmProduto = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    TipoProduto = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    ValorSolicitado = table.Column<decimal>(type: "NUMBER(18,2)", nullable: true),
                    Parcelas = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    VolumeMensalEstimado = table.Column<decimal>(type: "NUMBER(18,2)", nullable: true),
                    TaxaPercentual = table.Column<decimal>(type: "NUMBER(18,2)", nullable: true),
                    EmpresaConveniada = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: true),
                    SalarioMensal = table.Column<decimal>(type: "NUMBER(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.idProduto);
                });

            migrationBuilder.CreateTable(
                name: "Contratacoes",
                columns: table => new
                {
                    idContratacao = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    idCliente = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    idAgencia = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    idProduto = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    TipoProduto = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    MensagemProcessamento = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    Tentativas = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratacoes", x => x.idContratacao);
                    table.ForeignKey(
                        name: "FK_Contratacoes_AgenciaBanco_idAgencia",
                        column: x => x.idAgencia,
                        principalTable: "AgenciaBanco",
                        principalColumn: "idAgencia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contratacoes_ClientesBanco_idCliente",
                        column: x => x.idCliente,
                        principalTable: "ClientesBanco",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contratacoes_Produtos_idProduto",
                        column: x => x.idProduto,
                        principalTable: "Produtos",
                        principalColumn: "idProduto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientesBanco_CNPJ",
                table: "ClientesBanco",
                column: "CNPJ",
                unique: true,
                filter: "\"CNPJ\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesBanco_CPF",
                table: "ClientesBanco",
                column: "CPF",
                unique: true,
                filter: "\"CPF\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesBanco_idAgencia",
                table: "ClientesBanco",
                column: "idAgencia");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_idAgencia",
                table: "Contratacoes",
                column: "idAgencia");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_idCliente",
                table: "Contratacoes",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_idProduto",
                table: "Contratacoes",
                column: "idProduto");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientesBanco_AgenciaBanco_idAgencia",
                table: "ClientesBanco",
                column: "idAgencia",
                principalTable: "AgenciaBanco",
                principalColumn: "idAgencia",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientesBanco_AgenciaBanco_idAgencia",
                table: "ClientesBanco");

            migrationBuilder.DropTable(
                name: "Contratacoes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_ClientesBanco_CNPJ",
                table: "ClientesBanco");

            migrationBuilder.DropIndex(
                name: "IX_ClientesBanco_CPF",
                table: "ClientesBanco");

            migrationBuilder.DropIndex(
                name: "IX_ClientesBanco_idAgencia",
                table: "ClientesBanco");

            migrationBuilder.DropColumn(
                name: "CNPJ",
                table: "ClientesBanco");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "ClientesBanco");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "ClientesBanco");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "ClientesBanco");

            migrationBuilder.DropColumn(
                name: "TipoCliente",
                table: "ClientesBanco");

            migrationBuilder.DropColumn(
                name: "idAgencia",
                table: "ClientesBanco");

            migrationBuilder.AlterColumn<string>(
                name: "nmCliente",
                table: "ClientesBanco",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "nomeBanco",
                table: "Bancos",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Bancos",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "nmEndereco",
                table: "AgenciaBanco",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "cep",
                table: "AgenciaBanco",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(20)",
                oldMaxLength: 20);
        }
    }
}
