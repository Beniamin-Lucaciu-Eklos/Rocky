using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky.UI.Migrations
{
    /// <inheritdoc />
    public partial class addApplicationTypeToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ApplicationTypeId",
                table: "Products",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ApplicationTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
