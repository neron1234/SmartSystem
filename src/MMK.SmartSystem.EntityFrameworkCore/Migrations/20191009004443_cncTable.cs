using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMK.SmartSystem.Migrations
{
    public partial class cncTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpSettings_TenantId_Name",
                table: "AbpSettings");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageName",
                table: "AbpLanguageTexts",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AbpLanguages",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.CreateTable(
                name: "LaserLibrary_CuttingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MachiningDataGroupId = table.Column<int>(nullable: false),
                    ENo = table.Column<short>(nullable: false),
                    MachiningKindId = table.Column<int>(nullable: false),
                    NozzleKindId = table.Column<int>(nullable: false),
                    NozzleDiameter = table.Column<double>(nullable: false),
                    Feedrate = table.Column<double>(nullable: false),
                    Power = table.Column<short>(nullable: false),
                    Frequency = table.Column<short>(nullable: false),
                    Duty = table.Column<short>(nullable: false),
                    GasPressure = table.Column<double>(nullable: false),
                    GasId = table.Column<int>(nullable: false),
                    GasSettingTime = table.Column<int>(nullable: false),
                    StandardDisplacement = table.Column<double>(nullable: false),
                    Supple = table.Column<double>(nullable: false),
                    EdgeSlt = table.Column<short>(nullable: false),
                    ApprSlt = table.Column<short>(nullable: false),
                    PwrCtrl = table.Column<short>(nullable: false),
                    StandardDisplacement2 = table.Column<double>(nullable: false),
                    GapAxis = table.Column<string>(nullable: false),
                    BeamSpot = table.Column<double>(nullable: false),
                    FocalPosition = table.Column<double>(nullable: false),
                    LiftDistance = table.Column<double>(nullable: false),
                    PbPower = table.Column<short>(nullable: false),
                    Reserve1 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve2 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve3 = table.Column<string>(maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_CuttingData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_EdgeCuttingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MachiningDataGroupId = table.Column<int>(nullable: false),
                    ENo = table.Column<short>(nullable: false),
                    MachiningKindId = table.Column<int>(nullable: false),
                    NozzleKindId = table.Column<int>(nullable: false),
                    NozzleDiameter = table.Column<double>(nullable: false),
                    Angle = table.Column<double>(nullable: false),
                    Power = table.Column<short>(nullable: false),
                    Frequency = table.Column<short>(nullable: false),
                    Duty = table.Column<short>(nullable: false),
                    GasPressure = table.Column<double>(nullable: false),
                    GasId = table.Column<int>(nullable: false),
                    PiercingTime = table.Column<int>(nullable: false),
                    RecoveryDistance = table.Column<double>(nullable: false),
                    RecoveryFrequency = table.Column<short>(nullable: false),
                    RecoveryFeedrate = table.Column<double>(nullable: false),
                    RecoveryDuty = table.Column<short>(nullable: false),
                    Gap = table.Column<double>(nullable: false),
                    GapAxis = table.Column<string>(nullable: false),
                    BeamSpot = table.Column<double>(nullable: false),
                    FocalPosition = table.Column<double>(nullable: false),
                    LiftDistance = table.Column<double>(nullable: false),
                    PbPower = table.Column<short>(nullable: false),
                    Reserve1 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve2 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve3 = table.Column<string>(maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_EdgeCuttingData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_Gas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<short>(nullable: false),
                    Name_EN = table.Column<string>(maxLength: 100, nullable: true),
                    Name_CN = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_Gas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_MachiningDataGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    MaterialThickness = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_MachiningDataGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_MachiningKind",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_EN = table.Column<string>(maxLength: 100, nullable: true),
                    Name_CN = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_MachiningKind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_Material",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Name_EN = table.Column<string>(maxLength: 100, nullable: true),
                    Name_CN = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_NozzleKind",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_EN = table.Column<string>(maxLength: 100, nullable: true),
                    Name_CN = table.Column<string>(maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_NozzleKind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_PiercingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MachiningDataGroupId = table.Column<int>(nullable: false),
                    ENo = table.Column<short>(nullable: false),
                    MachiningKindId = table.Column<int>(nullable: false),
                    NozzleKindId = table.Column<int>(nullable: false),
                    NozzleDiameter = table.Column<double>(nullable: false),
                    Power = table.Column<short>(nullable: false),
                    Frequency = table.Column<short>(nullable: false),
                    Duty = table.Column<short>(nullable: false),
                    StepFrequency = table.Column<short>(nullable: false),
                    StepDuty = table.Column<short>(nullable: false),
                    StepTime = table.Column<short>(nullable: false),
                    StepQuantity = table.Column<short>(nullable: false),
                    PiercingTime = table.Column<int>(nullable: false),
                    GasPressure = table.Column<double>(nullable: false),
                    GasId = table.Column<int>(nullable: false),
                    GasSettingTime = table.Column<int>(nullable: false),
                    StandardDisplacement = table.Column<double>(nullable: false),
                    StandardDisplacement2 = table.Column<double>(nullable: false),
                    GapAxis = table.Column<string>(nullable: false),
                    BeamSpot = table.Column<double>(nullable: false),
                    FocalPosition = table.Column<double>(nullable: false),
                    LiftDistance = table.Column<double>(nullable: false),
                    PbPower = table.Column<short>(nullable: false),
                    Reserve1 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve2 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve3 = table.Column<string>(maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_PiercingData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaserLibrary_SlopeControlData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MachiningDataGroupId = table.Column<int>(nullable: false),
                    ENo = table.Column<short>(nullable: false),
                    MachiningKindId = table.Column<int>(nullable: false),
                    NozzleKindId = table.Column<int>(nullable: false),
                    NozzleDiameter = table.Column<double>(nullable: false),
                    PowerMin = table.Column<short>(nullable: false),
                    PowerSpeedZero = table.Column<short>(nullable: false),
                    FrequencyMin = table.Column<short>(nullable: false),
                    FrequencySpeedZero = table.Column<short>(nullable: false),
                    DutyMin = table.Column<short>(nullable: false),
                    DutySpeedZero = table.Column<short>(nullable: false),
                    FeedrateR = table.Column<double>(nullable: false),
                    PbPowerMin = table.Column<short>(nullable: false),
                    PbPowerSpeedZero = table.Column<short>(nullable: false),
                    GasPressMin = table.Column<short>(nullable: false),
                    GasPressSpeedZero = table.Column<short>(nullable: false),
                    BeamSpot = table.Column<double>(nullable: false),
                    FocalPosition = table.Column<double>(nullable: false),
                    LiftDistance = table.Column<double>(nullable: false),
                    Reserve1 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve2 = table.Column<string>(maxLength: 100, nullable: true),
                    Reserve3 = table.Column<string>(maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserLibrary_SlopeControlData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_TenantId_Name_UserId",
                table: "AbpSettings",
                columns: new[] { "TenantId", "Name", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaserLibrary_CuttingData");

            migrationBuilder.DropTable(
                name: "LaserLibrary_EdgeCuttingData");

            migrationBuilder.DropTable(
                name: "LaserLibrary_Gas");

            migrationBuilder.DropTable(
                name: "LaserLibrary_MachiningDataGroup");

            migrationBuilder.DropTable(
                name: "LaserLibrary_MachiningKind");

            migrationBuilder.DropTable(
                name: "LaserLibrary_Material");

            migrationBuilder.DropTable(
                name: "LaserLibrary_NozzleKind");

            migrationBuilder.DropTable(
                name: "LaserLibrary_PiercingData");

            migrationBuilder.DropTable(
                name: "LaserLibrary_SlopeControlData");

            migrationBuilder.DropIndex(
                name: "IX_AbpSettings_TenantId_Name_UserId",
                table: "AbpSettings");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageName",
                table: "AbpLanguageTexts",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AbpLanguages",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_TenantId_Name",
                table: "AbpSettings",
                columns: new[] { "TenantId", "Name" });
        }
    }
}
