using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RenameForgetToForgot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "forgetPasswordResetToken",
                table: "account",
                newName: "forgotPasswordResetToken");

            migrationBuilder.RenameColumn(
                name: "expireDateForgetPasswordResetToken",
                table: "account",
                newName: "expireDateForgotPasswordResetToken");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "forgotPasswordResetToken",
                table: "account",
                newName: "forgetPasswordResetToken");

            migrationBuilder.RenameColumn(
                name: "expireDateForgotPasswordResetToken",
                table: "account",
                newName: "expireDateForgetPasswordResetToken");

            
        }
    }
}
