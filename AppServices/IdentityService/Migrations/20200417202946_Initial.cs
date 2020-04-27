using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityService.Migrations {
    public partial class Initial : Migration {
        protected override void Up (MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable (
                name: "Users",
                columns : table => new {
                    Id = table.Column<Guid> (nullable: false),
                        Email = table.Column<string> (nullable: true),
                        Password = table.Column<string> (nullable: true),
                        Salt = table.Column<string> (nullable: true),
                        Phone = table.Column<string> (nullable: true)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_Users", x => x.Id);
                });
        }

        protected override void Down (MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable (
                name: "Users");
        }
    }
}