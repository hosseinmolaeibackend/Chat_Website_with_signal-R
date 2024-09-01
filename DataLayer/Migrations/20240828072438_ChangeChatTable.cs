using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "ChatGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "ChatGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_ReceiverId",
                table: "ChatGroups",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroups_Users_ReceiverId",
                table: "ChatGroups",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroups_Users_ReceiverId",
                table: "ChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_ChatGroups_ReceiverId",
                table: "ChatGroups");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "ChatGroups");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "ChatGroups");
        }
    }
}
