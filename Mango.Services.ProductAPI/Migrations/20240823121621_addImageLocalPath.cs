using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class addImageLocalPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLocalPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ImageLocalPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ImageLocalPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ImageLocalPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ImageLocalPath",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLocalPath",
                table: "Products");
        }
    }
}
