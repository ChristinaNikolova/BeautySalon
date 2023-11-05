namespace BeautySalon.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddNewColumnToAnswerAndQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StylistId",
                table: "Questions",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Answers",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_StylistId",
                table: "Questions",
                column: "StylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ClientId",
                table: "Answers",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_ClientId",
                table: "Answers",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_StylistId",
                table: "Questions",
                column: "StylistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_ClientId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_StylistId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_StylistId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_ClientId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "StylistId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Answers");
        }
    }
}
