using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalR_Demo_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColToChatRoomUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatRoomUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatRoomUsers");
        }
    }
}
