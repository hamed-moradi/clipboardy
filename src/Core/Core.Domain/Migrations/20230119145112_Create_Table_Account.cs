using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace Core.Domain.Migrations {
  /// <inheritdoc />
  public partial class CreateTableAccount: Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "Account",
          columns: table => new {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            Password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            ProviderId = table.Column<int>(type: "integer", nullable: false),
            StatusId = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
            LastSignedinAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            InsertedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table => {
            table.PrimaryKey("PK_Account", x => x.Id);
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "Account");
    }
  }
}
