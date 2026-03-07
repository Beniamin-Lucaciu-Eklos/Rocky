
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky.UI.Migrations
{
    /// <inheritdoc />
    public partial class addProductImageNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(nameof(Models.Product.Image),
                "Products",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(nameof(Models.Product.Image),
              "Products",
              nullable: false);
        }
    }
}
