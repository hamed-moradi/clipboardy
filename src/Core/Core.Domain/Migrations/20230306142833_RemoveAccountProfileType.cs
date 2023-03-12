using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccountProfileType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_profile_account_profile_type_type_id",
                table: "account_profile");

            migrationBuilder.DropTable(
                name: "account_profile_type");

            migrationBuilder.DropIndex(
                name: "IX_account_profile_type_id",
                table: "account_profile");

            migrationBuilder.DropColumn(
                name: "type_id",
                table: "account_profile");

            migrationBuilder.AddColumn<string>(
                name: "profile_type",
                table: "account_profile",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_type",
                table: "account_profile");

            migrationBuilder.AddColumn<int>(
                name: "type_id",
                table: "account_profile",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "account_profile_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_profile_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_profile_type_id",
                table: "account_profile",
                column: "type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_account_profile_account_profile_type_type_id",
                table: "account_profile",
                column: "type_id",
                principalTable: "account_profile_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
