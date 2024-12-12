using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorshipPlatformAPI.Migrations
{
    public partial class profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "AvailableFor",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Expertise",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "Interests",
                table: "Profiles",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Goals",
                table: "Profiles",
                newName: "Phone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Profiles",
                newName: "Interests");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Profiles",
                newName: "Goals");

            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "Profiles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AvailableFor",
                table: "Profiles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Expertise",
                table: "Profiles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Profiles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
