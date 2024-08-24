using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeForgeinKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroups_Users_OwnerId",
                table: "ChatGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroups_Users_OwnerId",
                table: "ChatGroups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroups_Users_OwnerId",
                table: "ChatGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroups_Users_OwnerId",
                table: "ChatGroups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
