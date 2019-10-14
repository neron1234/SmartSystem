using Microsoft.EntityFrameworkCore.Migrations;

namespace MMK.SmartSystem.Migrations
{
    public partial class cncCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachiningKindId",
                table: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropColumn(
                name: "NozzleKindId",
                table: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropColumn(
                name: "Reserve1",
                table: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropColumn(
                name: "Reserve2",
                table: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropColumn(
                name: "Reserve3",
                table: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropColumn(
                name: "GasId",
                table: "LaserLibrary_PiercingData");

            migrationBuilder.DropColumn(
                name: "MachiningKindId",
                table: "LaserLibrary_PiercingData");

            migrationBuilder.DropColumn(
                name: "NozzleKindId",
                table: "LaserLibrary_PiercingData");

            migrationBuilder.DropColumn(
                name: "GasId",
                table: "LaserLibrary_EdgeCuttingData");

            migrationBuilder.DropColumn(
                name: "MachiningKindId",
                table: "LaserLibrary_EdgeCuttingData");

            migrationBuilder.DropColumn(
                name: "NozzleKindId",
                table: "LaserLibrary_EdgeCuttingData");

            migrationBuilder.DropColumn(
                name: "GasId",
                table: "LaserLibrary_CuttingData");

            migrationBuilder.DropColumn(
                name: "MachiningKindId",
                table: "LaserLibrary_CuttingData");

            migrationBuilder.DropColumn(
                name: "NozzleKindId",
                table: "LaserLibrary_CuttingData");

            migrationBuilder.AddColumn<short>(
                name: "MachiningKindCode",
                table: "LaserLibrary_SlopeControlData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "NozzleKindCode",
                table: "LaserLibrary_SlopeControlData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "GasCode",
                table: "LaserLibrary_PiercingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "MachiningKindCode",
                table: "LaserLibrary_PiercingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "NozzleKindCode",
                table: "LaserLibrary_PiercingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Code",
                table: "LaserLibrary_NozzleKind",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Code",
                table: "LaserLibrary_MachiningKind",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "GasCode",
                table: "LaserLibrary_EdgeCuttingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "MachiningKindCode",
                table: "LaserLibrary_EdgeCuttingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "NozzleKindCode",
                table: "LaserLibrary_EdgeCuttingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "GasCode",
                table: "LaserLibrary_CuttingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "MachiningKindCode",
                table: "LaserLibrary_CuttingData",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "NozzleKindCode",
                table: "LaserLibrary_CuttingData",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachiningKindCode",
                table: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropColumn(
                name: "NozzleKindCode",
                table: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropColumn(
                name: "GasCode",
                table: "LaserLibrary_PiercingData");

            migrationBuilder.DropColumn(
                name: "MachiningKindCode",
                table: "LaserLibrary_PiercingData");

            migrationBuilder.DropColumn(
                name: "NozzleKindCode",
                table: "LaserLibrary_PiercingData");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "LaserLibrary_NozzleKind");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "LaserLibrary_MachiningKind");

            migrationBuilder.DropColumn(
                name: "GasCode",
                table: "LaserLibrary_EdgeCuttingData");

            migrationBuilder.DropColumn(
                name: "MachiningKindCode",
                table: "LaserLibrary_EdgeCuttingData");

            migrationBuilder.DropColumn(
                name: "NozzleKindCode",
                table: "LaserLibrary_EdgeCuttingData");

            migrationBuilder.DropColumn(
                name: "GasCode",
                table: "LaserLibrary_CuttingData");

            migrationBuilder.DropColumn(
                name: "MachiningKindCode",
                table: "LaserLibrary_CuttingData");

            migrationBuilder.DropColumn(
                name: "NozzleKindCode",
                table: "LaserLibrary_CuttingData");

            migrationBuilder.AddColumn<int>(
                name: "MachiningKindId",
                table: "LaserLibrary_SlopeControlData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NozzleKindId",
                table: "LaserLibrary_SlopeControlData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reserve1",
                table: "LaserLibrary_SlopeControlData",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reserve2",
                table: "LaserLibrary_SlopeControlData",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reserve3",
                table: "LaserLibrary_SlopeControlData",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GasId",
                table: "LaserLibrary_PiercingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MachiningKindId",
                table: "LaserLibrary_PiercingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NozzleKindId",
                table: "LaserLibrary_PiercingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GasId",
                table: "LaserLibrary_EdgeCuttingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MachiningKindId",
                table: "LaserLibrary_EdgeCuttingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NozzleKindId",
                table: "LaserLibrary_EdgeCuttingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GasId",
                table: "LaserLibrary_CuttingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MachiningKindId",
                table: "LaserLibrary_CuttingData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NozzleKindId",
                table: "LaserLibrary_CuttingData",
                nullable: false,
                defaultValue: 0);
        }
    }
}
