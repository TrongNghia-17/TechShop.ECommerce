using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace TechShop.ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFullTextSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Products",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector(\r\n    'simple',\r\n    coalesce(\"Name\", '') || ' ' ||\r\n    coalesce(\"Summary\", '') || ' ' ||\r\n    coalesce(\"Description\", '')\r\n)",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DateCreated_Id",
                table: "Products",
                columns: new[] { "DateCreated", "Id" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SearchVector",
                table: "Products",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_DateCreated_Id",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SearchVector",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Products");
        }
    }
}
