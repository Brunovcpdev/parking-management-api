using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estacionamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndiceUnicoTipoVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TabelaPrecos_TipoVeiculo",
                table: "TabelaPrecos",
                column: "TipoVeiculo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TabelaPrecos_TipoVeiculo",
                table: "TabelaPrecos");
        }
    }
}
