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
          name: "account",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            providerid = table.Column<int>(name: "provider_id", type: "integer", nullable: false),
            lastsignedinat = table.Column<DateTime>(name: "last_signedin_at", type: "timestamp with time zone", nullable: true),
            insertedat = table.Column<DateTime>(name: "inserted_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
            statusid = table.Column<int>(name: "status_id", type: "integer", nullable: false, defaultValue: 10)
          },
          constraints: table => {
            table.PrimaryKey("PK_account", x => x.id);
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "account");
    }
  }
}
