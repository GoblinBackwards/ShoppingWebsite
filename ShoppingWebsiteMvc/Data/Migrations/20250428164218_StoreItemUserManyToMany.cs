using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsiteMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class StoreItemUserManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CustomerIdentityUserStoreItem",
                columns: table => new
                {
                    InCartOfId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemsInCartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIdentityUserStoreItem", x => new { x.InCartOfId, x.ItemsInCartId });
                    table.ForeignKey(
                        name: "FK_CustomerIdentityUserStoreItem_AspNetUsers_InCartOfId",
                        column: x => x.InCartOfId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerIdentityUserStoreItem_StoreItems_ItemsInCartId",
                        column: x => x.ItemsInCartId,
                        principalTable: "StoreItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIdentityUserStoreItem_ItemsInCartId",
                table: "CustomerIdentityUserStoreItem",
                column: "ItemsInCartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerIdentityUserStoreItem");

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
    }
}
