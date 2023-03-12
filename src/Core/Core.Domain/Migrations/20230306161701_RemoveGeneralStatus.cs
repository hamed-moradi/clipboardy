using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGeneralStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "general_status");

            migrationBuilder.DropColumn(
                name: "status_id",
                table: "clipboard");

            migrationBuilder.DropColumn(
                name: "status_id",
                table: "account_profile");

            migrationBuilder.DropColumn(
                name: "status_id",
                table: "account_device");

            migrationBuilder.DropColumn(
                name: "status_id",
                table: "account");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "clipboard",
                type: "text",
                nullable: true,
                defaultValue: "ACTIVE");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "account_profile",
                type: "text",
                nullable: true,
                defaultValue: "ACTIVE");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "account_device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "account",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "clipboard");

            migrationBuilder.DropColumn(
                name: "status",
                table: "account_profile");

            migrationBuilder.DropColumn(
                name: "status",
                table: "account_device");

            migrationBuilder.DropColumn(
                name: "status",
                table: "account");

            migrationBuilder.AddColumn<int>(
                name: "status_id",
                table: "clipboard",
                type: "integer",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "status_id",
                table: "account_profile",
                type: "integer",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "status_id",
                table: "account_device",
                type: "integer",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "status_id",
                table: "account",
                type: "integer",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.CreateTable(
                name: "general_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_general_status", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_general_status_title",
                table: "general_status",
                column: "title",
                unique: true);
        }
    }
}
