using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddEpireDateAndForgetPasswordTokenToEntityAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expireDateForgetPasswordResetToken",
                table: "account",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "forgetPasswordResetToken",
                table: "account",
                type: "text",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expireDateForgetPasswordResetToken",
                table: "account");

            migrationBuilder.DropColumn(
                name: "forgetPasswordResetToken",
                table: "account");

        }
    }
}
