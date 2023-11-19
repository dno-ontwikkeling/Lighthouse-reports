using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightHouseReports.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWebsiteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Websites",
                newName: "WebisteUrl");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Websites",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SiteMaps",
                table: "Websites",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "SiteMaps",
                table: "Websites");

            migrationBuilder.RenameColumn(
                name: "WebisteUrl",
                table: "Websites",
                newName: "Url");
        }
    }
}
