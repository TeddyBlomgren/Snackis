using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snackis.Migrations
{
    /// <inheritdoc />
    public partial class fixedReport3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Reports");

            migrationBuilder.AddColumn<bool>(
                name: "IsHandled",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHandled",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Reports",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
