using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky.UI.Migrations
{
    /// <inheritdoc />
    public partial class mandatoryApplicationTypeForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationTypeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationTypeId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id");
        }
    }
}
