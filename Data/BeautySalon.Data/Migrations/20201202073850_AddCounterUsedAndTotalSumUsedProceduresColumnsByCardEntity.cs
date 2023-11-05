namespace BeautySalon.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddCounterUsedAndTotalSumUsedProceduresColumnsByCardEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndEnd",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "CounterUsed",
                table: "Cards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TotalSumUsedProcedures",
                table: "Cards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CounterUsed",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "TotalSumUsedProcedures",
                table: "Cards");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndEnd",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
