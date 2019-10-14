using Microsoft.EntityFrameworkCore.Migrations;

namespace MMK.SmartSystem.Migrations
{
    public partial class cncCreatorCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "LaserLibrary_MachiningDataGroup");

            migrationBuilder.AddColumn<short>(
                name: "MaterialCode",
                table: "LaserLibrary_MachiningDataGroup",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialCode",
                table: "LaserLibrary_MachiningDataGroup");

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "LaserLibrary_MachiningDataGroup",
                nullable: false,
                defaultValue: 0);
        }
    }
}
