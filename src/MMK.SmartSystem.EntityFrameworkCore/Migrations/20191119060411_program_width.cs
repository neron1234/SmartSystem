using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMK.SmartSystem.Migrations
{
    public partial class program_width : Migration
    {
    
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
              name: "PlateSize_W",
              table: "LaserProgram_ProgramComment",
              defaultValue: 0,
              nullable: true);
            migrationBuilder.AddColumn<double>(
             name: "PlateSize_H",
             table: "LaserProgram_ProgramComment",
             defaultValue: 0,
             nullable: true);
            migrationBuilder.AddColumn<double>(
             name: "UsedPlateSize_W",
             table: "LaserProgram_ProgramComment",
             defaultValue: 0,
             nullable: true);
            migrationBuilder.AddColumn<double>(
             name: "UsedPlateSize_H",
             table: "LaserProgram_ProgramComment",
             defaultValue: 0,
             nullable: true);
            migrationBuilder.AddColumn<double>(
             name: "Max_X",
             table: "LaserProgram_ProgramComment",
             defaultValue: 0,
             nullable: true);
            migrationBuilder.AddColumn<double>(
             name: "Max_Y",
             table: "LaserProgram_ProgramComment",
             defaultValue: 0,
             nullable: true);
            migrationBuilder.AddColumn<double>(
             name: "Min_X",
             table: "LaserProgram_ProgramComment",
             defaultValue: 0,
             nullable: true);
            migrationBuilder.AddColumn<double>(
             name: "Min_Y",
             table: "LaserProgram_ProgramComment",
             defaultValue: 0,
             nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "PlateSize_W",
            table: "LaserProgram_ProgramComment");

            migrationBuilder.DropColumn(
            name: "PlateSize_H",
            table: "LaserProgram_ProgramComment"); 

            migrationBuilder.DropColumn(
             name: "UsedPlateSize_W",
             table: "LaserProgram_ProgramComment");

            migrationBuilder.DropColumn(
             name: "UsedPlateSize_H",
             table: "LaserProgram_ProgramComment"); 

            migrationBuilder.DropColumn(
             name: "Max_X",
             table: "LaserProgram_ProgramComment");

            migrationBuilder.DropColumn(
            name: "Max_Y",
            table: "LaserProgram_ProgramComment");

            migrationBuilder.DropColumn(
            name: "Min_X",
            table: "LaserProgram_ProgramComment");

            migrationBuilder.DropColumn(
            name: "Min_Y",
            table: "LaserProgram_ProgramComment");
        }
    }
}
