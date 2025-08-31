﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumnInCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "PFM",
                table: "Cards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "PFM",
                table: "Cards");
        }
    }
}
