using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AstuteTec.Models.Migrations
{
    public partial class _20201230_yankejun_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "NJQHWATER");

            migrationBuilder.CreateTable(
                name: "Dict",
                schema: "NJQHWATER",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dict", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                schema: "NJQHWATER",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Account = table.Column<string>(maxLength: 30, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    Cellphone = table.Column<string>(maxLength: 11, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: false),
                    Removed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictItem",
                schema: "NJQHWATER",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DictionaryId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(maxLength: 100, nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: true),
                    NumericalOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictItem_Dict_DictionaryId",
                        column: x => x.DictionaryId,
                        principalSchema: "NJQHWATER",
                        principalTable: "Dict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictItem_DictionaryId",
                schema: "NJQHWATER",
                table: "DictItem",
                column: "DictionaryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictItem",
                schema: "NJQHWATER");

            migrationBuilder.DropTable(
                name: "UserInfo",
                schema: "NJQHWATER");

            migrationBuilder.DropTable(
                name: "Dict",
                schema: "NJQHWATER");
        }
    }
}
