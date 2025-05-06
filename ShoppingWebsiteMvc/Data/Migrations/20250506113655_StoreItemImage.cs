using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsiteMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class StoreItemImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "StoreItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "StoreItems");
        }
    }
}
