using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GrupoJap.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoCombustivel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descritivo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCombustivel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnoFabrico = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    TipoCombustivelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veiculo_TipoCombustivel_TipoCombustivelId",
                        column: x => x.TipoCombustivelId,
                        principalTable: "TipoCombustivel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContratoAluguer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuilometragemInicial = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoAluguer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoAluguer_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContratoAluguer_Veiculo_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "Id", "CartaConducao", "Email", "NomeCompleto", "Telefone" },
                values: new object[] { new Guid("d74e1db1-beee-4fbc-bd53-82f1dab5b609"), true, "luisousa2002@gmail.com", "Luis Borges Sousa", "925195028" });

            migrationBuilder.InsertData(
                table: "TipoCombustivel",
                columns: new[] { "Id", "Descritivo" },
                values: new object[,]
                {
                    { new Guid("7a9737c8-2a6e-4ad6-b480-44ba3502dc2e"), "Diesel" },
                    { new Guid("85bfdd65-9bb3-4a79-bb26-d171fdca6722"), "Gasolina" },
                    { new Guid("b1b992fb-c080-4e4a-b138-1e3f1352c96f"), "GPL" }
                });

            migrationBuilder.InsertData(
                table: "Veiculo",
                columns: new[] { "Id", "AnoFabrico", "Estado", "Marca", "Matricula", "Modelo", "TipoCombustivelId" },
                values: new object[] { new Guid("d275f7a7-e4de-41ef-a79f-56997c37b2fc"), 2010, false, "Ford", "88-JZ-33", "Fiesta", new Guid("7a9737c8-2a6e-4ad6-b480-44ba3502dc2e") });

            migrationBuilder.InsertData(
                table: "ContratoAluguer",
                columns: new[] { "Id", "ClienteId", "DataFim", "DataInicio", "QuilometragemInicial", "VeiculoId" },
                values: new object[] { new Guid("a138d50c-9853-48ed-9358-80c5b63b3248"), new Guid("d74e1db1-beee-4fbc-bd53-82f1dab5b609"), new DateTime(2025, 1, 2, 21, 49, 14, 832, DateTimeKind.Local).AddTicks(6034), new DateTime(2024, 12, 28, 21, 49, 14, 831, DateTimeKind.Local).AddTicks(24), 0, new Guid("d275f7a7-e4de-41ef-a79f-56997c37b2fc") });

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAluguer_ClienteId",
                table: "ContratoAluguer",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAluguer_VeiculoId",
                table: "ContratoAluguer",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_TipoCombustivelId",
                table: "Veiculo",
                column: "TipoCombustivelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContratoAluguer");

            migrationBuilder.DropTable(
                name: "Veiculo");

            migrationBuilder.DropTable(
                name: "TipoCombustivel");

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: new Guid("d74e1db1-beee-4fbc-bd53-82f1dab5b609"));
        }
    }
}
