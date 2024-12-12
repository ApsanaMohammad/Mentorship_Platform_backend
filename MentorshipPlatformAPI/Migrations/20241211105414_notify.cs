using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorshipPlatformAPI.Migrations
{
    public partial class notify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Profiles",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MentorshipRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MentorId = table.Column<int>(type: "int", nullable: false),
                    MenteeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorshipRequests", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorshipRequests");

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Skills",
                keyValue: null,
                column: "Skills",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Profiles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
