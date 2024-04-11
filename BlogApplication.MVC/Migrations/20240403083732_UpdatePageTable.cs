using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace BlogApplication.MVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "Name",
            table: "Pages",
            newName: "Title",
            schema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.RenameColumn(
           name: "Title",
           table: "Pages",
           newName: "Name",
           schema: "dbo.BlogApplication");
        }
    }
}
