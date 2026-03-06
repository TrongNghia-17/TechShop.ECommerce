using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace TechShop.ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFullTextAndConcurrencyFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_SearchVector",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RowVersion",
                table: "Products",
                type: "integer",
                rowVersion: true,
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Products",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector(\r\n    'simple',\r\n    coalesce(\"Name\", '') || ' ' ||\r\n    coalesce(\"Summary\", '') || ' ' ||\r\n    coalesce(\"Description\", '')\r\n)",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SearchVector",
                table: "Products",
                column: "SearchVector",
                filter: "\"IsDeleted\" = false")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }
    }
}
