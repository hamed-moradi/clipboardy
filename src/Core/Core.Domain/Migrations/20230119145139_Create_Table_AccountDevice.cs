using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace Core.Domain.Migrations {
  /// <inheritdoc />
  public partial class CreateTableAccountDevice: Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "AccountDevice",
          columns: table => new {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            AccountId = table.Column<int>(type: "integer", nullable: false),
            DeviceId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
            DeviceName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
            DeviceType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
            StatusId = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
            InsertedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table => {
            table.PrimaryKey("PK_AccountDevice", x => x.Id);
            table.ForeignKey(
                      name: "FK_AccountDevice_Account_AccountId",
                      column: x => x.AccountId,
                      principalTable: "Account",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_AccountDevice_AccountId",
          table: "AccountDevice",
          column: "AccountId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "AccountDevice");
    }
  }
}
