using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCatalogue.Data.Migrations
{
    public partial class Addedsoftdeletevaluetogames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Games");
        }
    }
}
