using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsiteMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class CustomerIdentity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerIdentityUserId",
                table: "StoreItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_CustomerIdentityUserId",
                table: "StoreItems",
                column: "CustomerIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItems_AspNetUsers_CustomerIdentityUserId",
                table: "StoreItems",
                column: "CustomerIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreItems_AspNetUsers_CustomerIdentityUserId",
                table: "StoreItems");

            migrationBuilder.DropIndex(
                name: "IX_StoreItems_CustomerIdentityUserId",
                table: "StoreItems");

            migrationBuilder.DropColumn(
                name: "CustomerIdentityUserId",
                table: "StoreItems");
        }
    }
}
