using Microsoft.EntityFrameworkCore.Migrations;

namespace HashMaker5.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordHashes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashSha256 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashSha384 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashSha512 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHashes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NextPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfProcessedPasswords = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordHashes");

            migrationBuilder.DropTable(
                name: "ProcessingInfo");
        }
    }
}
