using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleColumnInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role",
                schema: "PFM",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                schema: "PFM",
                table: "Users");
        }
    }
}
