using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsiteMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class CartItemsLinkTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerIdentityUserStoreItem");

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => new { x.CustomerId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_CartItem_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_StoreItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "StoreItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ItemId",
                table: "CartItem",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

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
    }
}
