using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Migracionparasimuladorderanking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MacroIndicadorId",
                table: "Indicadores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SimulacionMacroindicadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MacroindicadorId = table.Column<int>(type: "int", nullable: false),
                    PesoSimulacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulacionMacroindicadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimulacionMacroindicadores_MacroIndicadores_MacroindicadorId",
                        column: x => x.MacroindicadorId,
                        principalTable: "MacroIndicadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Indicadores_MacroIndicadorId",
                table: "Indicadores",
                column: "MacroIndicadorId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulacionMacroindicadores_MacroindicadorId",
                table: "SimulacionMacroindicadores",
                column: "MacroindicadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Indicadores_MacroIndicadores_MacroIndicadorId",
                table: "Indicadores",
                column: "MacroIndicadorId",
                principalTable: "MacroIndicadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Indicadores_MacroIndicadores_MacroIndicadorId",
                table: "Indicadores");

            migrationBuilder.DropTable(
                name: "SimulacionMacroindicadores");

            migrationBuilder.DropIndex(
                name: "IX_Indicadores_MacroIndicadorId",
                table: "Indicadores");

            migrationBuilder.DropColumn(
                name: "MacroIndicadorId",
                table: "Indicadores");
        }
    }
}
