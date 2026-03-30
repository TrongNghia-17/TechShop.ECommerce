-- techshop_seed_data.sql
-- Bước 1: Xóa sạch dữ liệu cũ
TRUNCATE TABLE "ProductVectors" CASCADE;
TRUNCATE TABLE "Products" CASCADE;
TRUNCATE TABLE "Categories" CASCADE;

-- Bước 2: Tạo Danh mục (Dùng gen_random_uuid() cho Postgres)
INSERT INTO "Categories" ("Id", "Name", "Description", "DateCreated", "DateModified", "CreatedBy", "ModifiedBy")
VALUES 
('29d69f54-9781-46ba-a592-358009f3dc2b', 'Laptop', 'Máy tính xách tay làm việc và giải trí di động.', NOW(), NOW(), '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000'),
('8511ddde-8e89-4bf8-b26d-58f5eb61e81e', 'Smartphone', 'Điện thoại thông minh cá nhân.', NOW(), NOW(), '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000'),
('ece0bf6d-0bf5-4025-aa83-27a60bc6f918', 'Phụ kiện', 'Thiết bị ngoại vi và linh kiện hỗ trợ.', NOW(), NOW(), '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000');

-- Bước 3: Thêm Sản phẩm chuyên nghiệp
INSERT INTO "Products" ("Id", "Name", "Summary", "Description", "Price", "StockQuantity", "CategoryId", "IsDeleted", "DateCreated", "CreatedBy")
VALUES 
-- LAPTOPS
(gen_random_uuid(), 'MacBook Pro 16 M3 Max', 'Siêu phẩm cho chuyên gia.', 'Trang bị chip M3 Max cực mạnh cho thiết kế đồ họa và lập trình chuyên sâu.', 3499.00, 10, '29d69f54-9781-46ba-a592-358009f3dc2b', false, NOW(), '00000000-0000-0000-0000-000000000000'),
(gen_random_uuid(), 'Dell XPS 15 9530', 'Đẳng cấp doanh nhân.', 'Màn hình OLED 3.5K sắc nét, thiết kế nhôm nguyên khối sang trọng.', 2199.00, 15, '29d69f54-9781-46ba-a592-358009f3dc2b', false, NOW(), '00000000-0000-0000-0000-000000000000'),
(gen_random_uuid(), 'ThinkPad X1 Carbon Gen 11', 'Độ bền huyền thoại.', 'Bàn phím tốt nhất thế giới với trọng lượng siêu nhẹ cho doanh chủ hay di chuyển.', 1850.00, 20, '29d69f54-9781-46ba-a592-358009f3dc2b', false, NOW(), '00000000-0000-0000-0000-000000000000'),
(gen_random_uuid(), 'ROG Zephyrus G14 2024', 'Laptop Gaming tốt nhất.', 'Gọn nhẹ nhưng vẫn sở hữu card đồ họa mạnh mẽ cho game thủ và creator.', 1599.00, 12, '29d69f54-9781-46ba-a592-358009f3dc2b', false, NOW(), '00000000-0000-0000-0000-000000000000'),

-- SMARTPHONES
(gen_random_uuid(), 'iPhone 15 Pro Max', 'Đỉnh cao Titanium.', 'Camera zoom quang học 5x cùng viền titan nhẹ và bền bỉ.', 1199.00, 30, '8511ddde-8e89-4bf8-b26d-58f5eb61e81e', false, NOW(), '00000000-0000-0000-0000-000000000000'),
(gen_random_uuid(), 'Samsung Galaxy S24 Ultra', 'AI cho kỷ nguyên mới.', 'Tích hợp bút S-Pen và tính năng dịch thuật AI trực tiếp qua cuộc gọi.', 1299.00, 25, '8511ddde-8e89-4bf8-b26d-58f5eb61e81e', false, NOW(), '00000000-0000-0000-0000-000000000000'),
(gen_random_uuid(), 'Google Pixel 8 Pro', 'Nhiếp ảnh thuần khiết.', 'Trải nghiệm Android mượt mà nhất với thuật toán xử lý ảnh hàng đầu của Google.', 999.00, 18, '8511ddde-8e89-4bf8-b26d-58f5eb61e81e', false, NOW(), '00000000-0000-0000-0000-000000000000'),

-- PHỤ KIỆN
(gen_random_uuid(), 'Sony WH-1000XM5', 'Chống ồn tuyệt đối.', 'Tai nghe chống ồn tốt nhất thế giới với thời lượng pin 30 giờ.', 399.00, 40, 'ece0bf6d-0bf5-4025-aa83-27a60bc6f918', false, NOW(), '00000000-0000-0000-0000-000000000000'),
(gen_random_uuid(), 'Logitech MX Master 3S', 'Vua của hiệu suất.', 'Chuột không dây yên tĩnh cao cấp, tối ưu cho lập trình viên và kế toán.', 99.00, 50, 'ece0bf6d-0bf5-4025-aa83-27a60bc6f918', false, NOW(), '00000000-0000-0000-0000-000000000000'),
(gen_random_uuid(), 'Keychron K2 V2', 'Bàn phím cơ quốc dân.', 'Thiết kế tối giản 75%, tương thích hoàn hảo cho cả Mac và Windows.', 79.00, 60, 'ece0bf6d-0bf5-4025-aa83-27a60bc6f918', false, NOW(), '00000000-0000-0000-0000-000000000000');
