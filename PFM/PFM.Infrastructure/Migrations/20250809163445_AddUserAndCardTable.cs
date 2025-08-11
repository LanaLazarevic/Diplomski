using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "card_id",
                schema: "PFM",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "PFM",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    birthday = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "PFM",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    card_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    available_amount = table.Column<double>(type: "numeric(20,2)", nullable: false),
                    reserved_amount = table.Column<double>(type: "numeric(20,2)", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cards_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "PFM",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_card_id",
                schema: "PFM",
                table: "Transactions",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_user_id",
                schema: "PFM",
                table: "Cards",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                schema: "PFM",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cards_card_id",
                schema: "PFM",
                table: "Transactions",
                column: "card_id",
                principalSchema: "PFM",
                principalTable: "Cards",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cards_card_id",
                schema: "PFM",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "PFM");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "PFM");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_card_id",
                schema: "PFM",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "card_id",
                schema: "PFM",
                table: "Transactions");
        }
    }
}
