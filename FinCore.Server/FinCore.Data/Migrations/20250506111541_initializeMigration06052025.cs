using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class initializeMigration06052025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Cards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Cards");
        }
    }
}
