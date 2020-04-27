using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations {
    public partial class InitParentChildRelationship : Migration {
        protected override void Up (MigrationBuilder migrationBuilder) {
            migrationBuilder.EnsureSchema (
                name: "public");

            migrationBuilder.CreateTable (
                name: "EntityStatus",
                schema: "public",
                columns : table => new {
                    EntityStatusId = table.Column<int> (nullable: false),
                        Value = table.Column<string> (nullable: true)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_EntityStatus", x => x.EntityStatusId);
                });

            migrationBuilder.CreateTable (
                name: "Parents",
                schema: "public",
                columns : table => new {
                    Id = table.Column<Guid> (nullable: false),
                        CreatedDate = table.Column<DateTime> (nullable: false),
                        ModifiedDate = table.Column<DateTime> (nullable: false),
                        EntityStatusId = table.Column<int> (nullable: false),
                        FirstName = table.Column<string> (nullable: true),
                        SecondName = table.Column<string> (nullable: true),
                        LastName = table.Column<string> (nullable: true),
                        BirthDay = table.Column<DateTime> (nullable: false)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_Parents", x => x.Id);
                    table.ForeignKey (
                        name: "FK_Parents_EntityStatus_EntityStatusId",
                        column : x => x.EntityStatusId,
                        principalSchema: "public",
                        principalTable: "EntityStatus",
                        principalColumn: "EntityStatusId",
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable (
                name: "Children",
                schema: "public",
                columns : table => new {
                    Id = table.Column<Guid> (nullable: false),
                        CreatedDate = table.Column<DateTime> (nullable: false),
                        ModifiedDate = table.Column<DateTime> (nullable: false),
                        EntityStatusId = table.Column<int> (nullable: false),
                        FirstName = table.Column<string> (nullable: true),
                        SecondName = table.Column<string> (nullable: true),
                        LastName = table.Column<string> (nullable: true),
                        BirthDay = table.Column<DateTime> (nullable: false),
                        ParentId = table.Column<Guid> (nullable: false)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_Children", x => x.Id);
                    table.ForeignKey (
                        name: "FK_Children_EntityStatus_EntityStatusId",
                        column : x => x.EntityStatusId,
                        principalSchema: "public",
                        principalTable: "EntityStatus",
                        principalColumn: "EntityStatusId",
                        onDelete : ReferentialAction.Cascade);
                    table.ForeignKey (
                        name: "FK_Children_Parents_ParentId",
                        column : x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData (
                schema: "public",
                table: "EntityStatus",
                columns : new [] { "EntityStatusId", "Value" },
                values : new object[, ] { { 1, "Active" }, { 2, "Inactive" }
                });

            migrationBuilder.CreateIndex (
                name: "IX_Children_EntityStatusId",
                schema: "public",
                table: "Children",
                column: "EntityStatusId");

            migrationBuilder.CreateIndex (
                name: "IX_Children_ParentId",
                schema: "public",
                table: "Children",
                column: "ParentId");

            migrationBuilder.CreateIndex (
                name: "IX_Parents_EntityStatusId",
                schema: "public",
                table: "Parents",
                column: "EntityStatusId");
        }

        protected override void Down (MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable (
                name: "Children",
                schema: "public");

            migrationBuilder.DropTable (
                name: "Parents",
                schema: "public");

            migrationBuilder.DropTable (
                name: "EntityStatus",
                schema: "public");
        }
    }
}