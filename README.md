# 🛒 TechShop E-Commerce System

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Architecture](https://img.shields.io/badge/Architecture-Clean-brightgreen)](https://github.com/TrongNghia-17/TechShop.ECommerce)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📌 Tổng quan dự án (Overview)
**TechShop.ECommerce** là một hệ thống backend bán hàng trực tuyến được xây dựng dựa trên hệ sinh thái **.NET 8**. Dự án tập trung vào việc áp dụng **Clean Architecture** để đảm bảo tính dễ bảo trì, mở rộng và kiểm thử (testability).

Đây là dự án tâm huyết của mình nhằm thực hành các tư duy lập trình hiện đại và quy trình quản lý mã nguồn chuyên nghiệp.

---

## 🏗️ Kiến trúc hệ thống (Architecture)
Dự án tuân thủ nghiêm ngặt mô hình **Clean Architecture (Onion Architecture)** với 4 lớp tách biệt:

* **TechShop.ECommerce.Domain**: Lõi của ứng dụng, chứa các thực thể (Entities), giá trị (Value Objects) và các Interface cho Repository. Không phụ thuộc vào thư viện bên ngoài.
* **TechShop.ECommerce.Application**: Chứa logic nghiệp vụ (Use Cases), DTOs, Mapping và các Validator. Phụ thuộc vào lớp Domain.
* **TechShop.ECommerce.Infrastructure**: Cài đặt chi tiết cho việc truy cập dữ liệu (EF Core, SQL Server) và các dịch vụ bên thứ ba (Email, Payment).
* **TechShop.ECommerce.WebAPI**: Điểm vào của hệ thống (RESTful API), cấu hình Dependency Injection và Middleware.

---

## 🛠️ Công nghệ sử dụng (Tech Stack)

| Công nghệ | Mô tả |
| :--- | :--- |
| **Backend** | .NET 8 (C# 12) |
| **Database** | SQL Server |
| **ORM** | Entity Framework Core |
| **Security** | JWT Authentication, Identity Framework |
| **Mapping** | AutoMapper |
| **Validation** | FluentValidation |
| **Documentation** | Swagger (OpenAPI) |

---

## 🚀 Tính năng chính (Key Features)

- [x] **Quản lý sản phẩm**: CRUD sản phẩm, phân loại danh mục.
- [x] **Giỏ hàng**: Thêm, sửa, xóa sản phẩm trong giỏ hàng.
- [ ] **Thanh toán**: Tích hợp cổng thanh toán (Đang phát triển).
- [ ] **Xác thực**: Đăng ký, đăng nhập và phân quyền bằng JWT.
- [ ] **Unit Testing**: Đảm bảo chất lượng logic nghiệp vụ.

---

## 📥 Hướng dẫn cài đặt (Installation)

Để chạy dự án này trên môi trường local, bạn thực hiện các bước sau:

1. **Clone project:**
   ```bash
   git clone [https://github.com/TrongNghia-17/TechShop.ECommerce.git](https://github.com/TrongNghia-17/TechShop.ECommerce.git)
