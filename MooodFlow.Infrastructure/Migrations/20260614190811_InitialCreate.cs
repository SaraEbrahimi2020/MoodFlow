using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MooodFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoodLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WakeUpTime = table.Column<int>(type: "int", nullable: false),
                    EnergyLevel = table.Column<int>(type: "int", nullable: false),
                    MorningMoodScore = table.Column<int>(type: "int", nullable: false),
                    Gratitude1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gratitude2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gratitude3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatWouldMakeTodayGreat1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatWouldMakeTodayGreat2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatWouldMakeTodayGreat3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DailyAffirmation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmazingThingsHappened1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmazingThingsHappened2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmazingThingsHappened3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HowCouldIHaveMadeTodayBetter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterIntake = table.Column<int>(type: "int", nullable: false),
                    BedTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoodLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoodLogs");
        }
    }
}
