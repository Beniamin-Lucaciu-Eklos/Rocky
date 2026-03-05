using Microsoft.EntityFrameworkCore.Migrations;
using Rocky.UI.Models;

#nullable disable

namespace Rocky.UI.Migrations
{
    /// <inheritdoc />
    public partial class RequiredApplicationTypeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>
                (name: nameof(ApplicationType.Name),
                 table: "ApplicationTypes",
                 nullable:false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>
                (name: nameof(ApplicationType.Name),
                 table: "ApplicationTypes",
                 nullable: true);
        }
    }
}
