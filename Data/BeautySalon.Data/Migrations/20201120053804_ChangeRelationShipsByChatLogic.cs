namespace BeautySalon.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeRelationShipsByChatLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChatGroups_AspNetUsers_ApplicationUserId",
                table: "UserChatGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChatGroups",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserChatGroups");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserChatGroups",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "UserChatGroups",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "UserChatGroups",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserChatGroups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserChatGroups",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserChatGroups",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "UserChatGroups",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChatGroups",
                table: "UserChatGroups",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatGroups_AdminId",
                table: "UserChatGroups",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatGroups_ClientId",
                table: "UserChatGroups",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatGroups_IsDeleted",
                table: "UserChatGroups",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatGroups_AspNetUsers_AdminId",
                table: "UserChatGroups",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatGroups_AspNetUsers_ClientId",
                table: "UserChatGroups",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChatGroups_AspNetUsers_AdminId",
                table: "UserChatGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChatGroups_AspNetUsers_ClientId",
                table: "UserChatGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChatGroups",
                table: "UserChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserChatGroups_AdminId",
                table: "UserChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserChatGroups_ClientId",
                table: "UserChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserChatGroups_IsDeleted",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserChatGroups");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserChatGroups");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserChatGroups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChatGroups",
                table: "UserChatGroups",
                columns: new[] { "ApplicationUserId", "ChatGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatGroups_AspNetUsers_ApplicationUserId",
                table: "UserChatGroups",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
