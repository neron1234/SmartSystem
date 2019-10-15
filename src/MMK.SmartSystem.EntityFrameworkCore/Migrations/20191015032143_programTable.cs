using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMK.SmartSystem.Migrations
{
    public partial class programTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LaserProgram_ProgramComment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    FullPath = table.Column<string>(maxLength: 100, nullable: true),
                    Size = table.Column<double>(nullable: false),
                    Material = table.Column<string>(maxLength: 100, nullable: true),
                    Thickness = table.Column<double>(nullable: false),
                    Gas = table.Column<string>(maxLength: 100, nullable: true),
                    FocalPosition = table.Column<double>(nullable: false),
                    NozzleKind = table.Column<string>(maxLength: 100, nullable: true),
                    NozzleDiameter = table.Column<double>(nullable: false),
                    PlateSize = table.Column<string>(maxLength: 100, nullable: true),
                    UsedPlateSize = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingDistance = table.Column<double>(nullable: false),
                    PiercingCount = table.Column<int>(nullable: false),
                    CuttingTime = table.Column<double>(nullable: false),
                    ThumbnaiType = table.Column<int>(nullable: false),
                    ThumbnaiInfo = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaserProgram_ProgramComment", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaserProgram_ProgramComment");
        }
    }
}
