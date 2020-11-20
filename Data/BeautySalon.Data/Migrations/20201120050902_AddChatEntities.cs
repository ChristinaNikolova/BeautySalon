namespace BeautySalon.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddChatEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ChatMessages",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ChatGroups",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserChatGroups",
                columns: table => new
                {
                    ChatGroupId = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChatGroups", x => new { x.ApplicationUserId, x.ChatGroupId });
                    table.ForeignKey(
                        name: "FK_UserChatGroups_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserChatGroups_ChatGroups_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "ChatGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_IsDeleted",
                table: "ChatGroups",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatGroups_ChatGroupId",
                table: "UserChatGroups",
                column: "ChatGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChatGroups");

            migrationBuilder.DropTable(
                name: "ChatGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000);
        }
    }
}
