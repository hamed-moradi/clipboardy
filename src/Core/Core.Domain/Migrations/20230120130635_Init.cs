using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace Core.Domain.Migrations {
  /// <inheritdoc />
  public partial class Init: Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "account",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            statusid = table.Column<int>(name: "status_id", type: "integer", nullable: false, defaultValue: 10),
            lastsignedinat = table.Column<DateTime>(name: "last_signedin_at", type: "timestamp with time zone", nullable: true),
            insertedat = table.Column<DateTime>(name: "inserted_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table => {
            table.PrimaryKey("PK_account", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "account_profile_type",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            title = table.Column<string>(type: "text", nullable: false),
            description = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_account_profile_type", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "content_type",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
            extension = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
            mimetype = table.Column<string>(name: "mime_type", type: "character varying(128)", maxLength: 128, nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_content_type", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "general_status",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            title = table.Column<string>(type: "text", nullable: false),
            description = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_general_status", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "account_device",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            accountid = table.Column<int>(name: "account_id", type: "integer", nullable: false),
            devicekey = table.Column<string>(name: "device_key", type: "character varying(64)", maxLength: 64, nullable: false),
            devicename = table.Column<string>(name: "device_name", type: "character varying(128)", maxLength: 128, nullable: false),
            devicetype = table.Column<string>(name: "device_type", type: "character varying(64)", maxLength: 64, nullable: false),
            statusid = table.Column<int>(name: "status_id", type: "integer", nullable: false, defaultValue: 10),
            insertedat = table.Column<DateTime>(name: "inserted_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table => {
            table.PrimaryKey("PK_account_device", x => x.id);
            table.ForeignKey(
                      name: "FK_account_device_account_account_id",
                      column: x => x.accountid,
                      principalTable: "account",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "account_profile",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            accountid = table.Column<int>(name: "account_id", type: "integer", nullable: false),
            typeid = table.Column<int>(name: "type_id", type: "integer", nullable: false),
            linkedkey = table.Column<string>(name: "linked_key", type: "character varying(64)", maxLength: 64, nullable: true),
            statusid = table.Column<int>(name: "status_id", type: "integer", nullable: false, defaultValue: 10),
            insertedat = table.Column<DateTime>(name: "inserted_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table => {
            table.PrimaryKey("PK_account_profile", x => x.id);
            table.ForeignKey(
                      name: "FK_account_profile_account_account_id",
                      column: x => x.accountid,
                      principalTable: "account",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_account_profile_account_profile_type_type_id",
                      column: x => x.typeid,
                      principalTable: "account_profile_type",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "clipboard",
          columns: table => new {
            id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            accountid = table.Column<int>(name: "account_id", type: "integer", nullable: false),
            deviceid = table.Column<int>(name: "device_id", type: "integer", nullable: false),
            typeid = table.Column<int>(name: "type_id", type: "integer", nullable: false),
            content = table.Column<string>(type: "text", nullable: false),
            statusid = table.Column<int>(name: "status_id", type: "integer", nullable: false, defaultValue: 10),
            insertedat = table.Column<DateTime>(name: "inserted_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table => {
            table.PrimaryKey("PK_clipboard", x => x.id);
            table.ForeignKey(
                      name: "FK_clipboard_account_account_id",
                      column: x => x.accountid,
                      principalTable: "account",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_clipboard_account_device_device_id",
                      column: x => x.deviceid,
                      principalTable: "account_device",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_clipboard_content_type_type_id",
                      column: x => x.typeid,
                      principalTable: "content_type",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_account_device_account_id",
          table: "account_device",
          column: "account_id");

      migrationBuilder.CreateIndex(
          name: "IX_account_profile_account_id_linked_key",
          table: "account_profile",
          columns: new[] { "account_id", "linked_key" },
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_account_profile_type_id",
          table: "account_profile",
          column: "type_id");

      migrationBuilder.CreateIndex(
          name: "IX_clipboard_account_id",
          table: "clipboard",
          column: "account_id");

      migrationBuilder.CreateIndex(
          name: "IX_clipboard_device_id",
          table: "clipboard",
          column: "device_id");

      migrationBuilder.CreateIndex(
          name: "IX_clipboard_type_id",
          table: "clipboard",
          column: "type_id");

      migrationBuilder.CreateIndex(
          name: "IX_content_type_extension",
          table: "content_type",
          column: "extension",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_general_status_title",
          table: "general_status",
          column: "title",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "account_profile");

      migrationBuilder.DropTable(
          name: "clipboard");

      migrationBuilder.DropTable(
          name: "general_status");

      migrationBuilder.DropTable(
          name: "account_profile_type");

      migrationBuilder.DropTable(
          name: "account_device");

      migrationBuilder.DropTable(
          name: "content_type");

      migrationBuilder.DropTable(
          name: "account");
    }
  }
}
