using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Data.Migrations
{
    public partial class AddColumnDateTimeToProcedureReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ProcedureReviews",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ProcedureReviews");
        }
    }
}
