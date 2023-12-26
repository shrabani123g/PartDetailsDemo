using Microsoft.EntityFrameworkCore.Migrations;

namespace PartDetailsDemo.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Part",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Part", x => x.PartId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Part");
        }
    }
}
