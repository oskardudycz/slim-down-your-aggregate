using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublishingHouse.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OptimisticConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Books");
        }
    }
}
