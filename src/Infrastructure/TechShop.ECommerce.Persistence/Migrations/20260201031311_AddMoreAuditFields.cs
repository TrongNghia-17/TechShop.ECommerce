using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechShop.ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Products",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Orders",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "OrderItems",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Categories",
                newName: "DateModified");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Products",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Orders",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "OrderItems",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Categories",
                newName: "UpdatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Description", "IsDeleted", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-performance laptops, desktops, and workstations", false, "Laptops & PCs", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Latest mobile devices and tablets", false, "Smartphones & Tablets", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Keyboards, mice, chargers, and more", false, "Accessories", null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "Notes", "OrderDate", "ShippingAddress", "Status", "TotalPrice", "UpdatedDate", "UserId" },
                values: new object[] { 1, new DateTime(2024, 1, 20, 10, 0, 0, 0, DateTimeKind.Utc), false, "Please deliver during office hours", new DateTime(2024, 1, 20, 10, 0, 0, 0, DateTimeKind.Utc), "123 Tech Street, Silicon Valley, CA", 3, 3598.99m, null, "user-001" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "Notes", "OrderDate", "ShippingAddress", "TotalPrice", "UpdatedDate", "UserId" },
                values: new object[] { 2, new DateTime(2024, 1, 25, 15, 30, 0, 0, DateTimeKind.Utc), false, "", new DateTime(2024, 1, 25, 15, 30, 0, 0, DateTimeKind.Utc), "456 Developer Lane, Seattle, WA", 1199.00m, null, "user-002" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsDeleted", "Name", "Price", "StockQuantity", "Summary", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "16-inch Liquid Retina XDR display, M3 Max chip, 36GB Unified Memory, 1TB SSD.", false, "MacBook Pro 16 M3 Max", 3499.00m, 20, "Apple's most powerful laptop", null },
                    { 2, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "15.6 FHD+ Display, Intel Core i9, NVIDIA RTX 4060, 32GB RAM, 1TB SSD.", false, "Dell XPS 15", 2199.00m, 15, "Premium Windows laptop", null },
                    { 3, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Titanium design, A17 Pro chip, 48MP Main camera, USB-C connector.", false, "iPhone 15 Pro Max", 1199.00m, 50, "The ultimate iPhone", null },
                    { 4, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Snapdragon 8 Gen 3, 200MP Camera, S Pen included, Titanium frame.", false, "Samsung Galaxy S24 Ultra", 1299.00m, 45, "Galaxy AI is here", null },
                    { 5, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "8K DPI track-on-glass sensor, Quiet Clicks, USB-C rechargeable.", false, "Logitech MX Master 3S", 99.99m, 100, "Performance Wireless Mouse", null },
                    { 6, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Industry-leading noise canceling, 30-hour battery life, Crystal clear calling.", false, "Sony WH-1000XM5", 348.00m, 30, "Wireless Noise Cancelling Headphones", null }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "OrderId", "ProductId", "Quantity", "UnitPrice", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 20, 10, 0, 0, 0, DateTimeKind.Utc), false, 1, 1, 1, 3499.00m, null },
                    { 2, new DateTime(2024, 1, 20, 10, 0, 0, 0, DateTimeKind.Utc), false, 1, 5, 1, 99.99m, null },
                    { 3, new DateTime(2024, 1, 25, 15, 30, 0, 0, DateTimeKind.Utc), false, 2, 3, 1, 1199.00m, null }
                });
        }
    }
}
