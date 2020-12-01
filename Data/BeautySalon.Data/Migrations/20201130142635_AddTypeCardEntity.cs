namespace BeautySalon.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddTypeCardEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Cards",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeCardId",
                table: "Cards",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.CreateTable(
                name: "TypeCards",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_TypeCardId",
                table: "Cards",
                column: "TypeCardId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeCards_IsDeleted",
                table: "TypeCards",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_TypeCards_TypeCardId",
                table: "Cards",
                column: "TypeCardId",
                principalTable: "TypeCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_TypeCards_TypeCardId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "TypeCards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_TypeCardId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "TypeCardId",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
