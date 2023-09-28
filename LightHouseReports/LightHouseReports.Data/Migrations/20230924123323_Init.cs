using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightHouseReports.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    LastRun = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    FoundUrls = table.Column<int>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebsitesReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WebsiteDataModelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsitesReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsitesReport_Websites_WebsiteDataModelId",
                        column: x => x.WebsiteDataModelId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrlReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Adres = table.Column<string>(type: "TEXT", nullable: false),
                    ReportId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlReports_WebsitesReport_ReportId",
                        column: x => x.ReportId,
                        principalTable: "WebsitesReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LighthouseResultDataModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Preset = table.Column<int>(type: "INTEGER", nullable: false),
                    Performance = table.Column<int>(type: "INTEGER", nullable: false),
                    Accessibility = table.Column<int>(type: "INTEGER", nullable: false),
                    BestPractices = table.Column<int>(type: "INTEGER", nullable: false),
                    Seo = table.Column<int>(type: "INTEGER", nullable: false),
                    UrlReportDataModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LighthouseResultDataModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LighthouseResultDataModel_UrlReports_UrlReportDataModelId",
                        column: x => x.UrlReportDataModelId,
                        principalTable: "UrlReports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LighthouseResultDataModel_UrlReportDataModelId",
                table: "LighthouseResultDataModel",
                column: "UrlReportDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UrlReports_ReportId",
                table: "UrlReports",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsitesReport_WebsiteDataModelId",
                table: "WebsitesReport",
                column: "WebsiteDataModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LighthouseResultDataModel");

            migrationBuilder.DropTable(
                name: "UrlReports");

            migrationBuilder.DropTable(
                name: "WebsitesReport");

            migrationBuilder.DropTable(
                name: "Websites");
        }
    }
}
