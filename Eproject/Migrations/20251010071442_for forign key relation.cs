using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eproject.Migrations
{
    /// <inheritdoc />
    public partial class forforignkeyrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryCatID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryCatID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryCatID",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CatID",
                table: "Products",
                column: "CatID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CatID",
                table: "Products",
                column: "CatID",
                principalTable: "Categories",
                principalColumn: "CatID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CatID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CatID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "CategoryCatID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryCatID",
                table: "Products",
                column: "CategoryCatID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryCatID",
                table: "Products",
                column: "CategoryCatID",
                principalTable: "Categories",
                principalColumn: "CatID");
        }
    }
}
