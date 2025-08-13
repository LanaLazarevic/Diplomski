using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJmbgColumnInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "jmbg",
                schema: "PFM",
                table: "Users",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "jmbg",
                schema: "PFM",
                table: "Users");
        }
    }
}
