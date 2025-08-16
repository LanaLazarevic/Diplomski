using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCardTypeAndMakeJmbgUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "card_type",
                schema: "PFM",
                table: "Cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_jmbg",
                schema: "PFM",
                table: "Users",
                column: "jmbg",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_jmbg",
                schema: "PFM",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "card_type",
                schema: "PFM",
                table: "Cards");
        }
    }
}
