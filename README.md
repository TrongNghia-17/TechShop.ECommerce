# 🛒 TechShop E-Commerce System

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Redis](https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=white)](https://redis.io/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean-brightgreen)](https://github.com/TrongNghia-17/TechShop.ECommerce)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📌 Tổng quan dự án (Overview)
**TechShop.ECommerce** là một giải pháp thương mại điện tử (E-commerce) hiện đại, mạnh mẽ, và có khả năng mở rộng cao, được xây dựng trên nền tảng **.NET 10**. Dự án áp dụng triệt để mô hình **Clean Architecture** kết hợp cùng các công nghệ tiên tiến nhất hiện nay như **PostgreSQL (với Pgvector)**, **Redis Hybrid Cache**, **Stripe API**, và **OpenTelemetry**.

Hệ thống được thiết kế không chỉ để xử lý các nghiệp vụ bán hàng cơ bản mà còn tích hợp các tính năng thông minh như tìm kiếm bằng vector (AI-powered similarity search), xử lý tác vụ ngầm (Background Jobs), và giám sát hệ thống toàn diện.

---

## 🏗️ Kiến trúc hệ thống (Architecture)
Dự án tuân thủ nghiêm ngặt mô hình **Clean Architecture (Onion Architecture)** giúp tách biệt logic nghiệp vụ khỏi các yếu tố hạ tầng:

*   **`Core.Domain`**: Chứa các thực thể cốt lõi (Entities), Value Objects, Domain Exceptions và các Interface. Không phụ thuộc vào bất kỳ thư viện bên ngoài nào.
*   **`Core.Application`**: Triển khai logic nghiệp vụ (Use Cases) thông qua CQRS (MediatR), validation (FluentValidation), và mapping.
*   **`Infrastructure.Persistence`**: Quản lý truy xuất dữ liệu với **EF Core** và **PostgreSQL**, tích hợp **Pgvector** cho các tính năng AI.
*   **`Infrastructure.Infrastructure`**: Cài đặt các dịch vụ bên thứ ba: Cổng thanh toán (Stripe), Email (SendGrid), Lưu trữ (Azure Blobs), Caching (Redis), và Job Scheduling (Hangfire).
*   **`API.TechShop.ECommerce.Api`**: Cổng giao tiếp RESTful API, quản lý xác thực và phân quyền (JWT), cùng với tài liệu hóa API bằng **Scalar**.

---

## 🛠️ Công nghệ sử dụng (Tech Stack)

### Core Technologies
- **Framework**: [.NET 10.0 (Latest)](https://dotnet.microsoft.com/download)
- **Database**: [PostgreSQL](https://www.postgresql.org/) + [Pgvector](https://github.com/pgvector/pgvector-dotnet)
- **ORM**: [Entity Framework Core 10.0](https://learn.microsoft.com/en-us/ef/core/)
- **Caching**: [Redis](https://redis.io/) với **Microsoft.Extensions.Caching.Hybrid**
- **Authentication**: JWT & ASP.NET Core Identity

### Third-party Integrations
- **Payment Gateway**: [Stripe](https://stripe.com/)
- **Background Jobs**: [Hangfire](https://www.hangfire.io/) (với PostgreSQL Storage)
- **Email Service**: [SendGrid](https://sendgrid.com/)
- **Storage**: Azure Blob Storage
- **PDF Generation**: [QuestPDF](https://www.questpdf.com/)
- **Image Processing**: [SixLabors.ImageSharp](https://sixlabors.com/products/imagesharp/)

### Observability & Logging
- **Logging**: [Serilog](https://serilog.net/) (Sinks: Console, File, Seq)
- **Telemetry**: [OpenTelemetry](https://opentelemetry.io/) (AspNetCore, Http, EFCore, Runtime)
- **API Documentation**: [Scalar](https://scalar.com/) (Thay thế cho Swagger truyền thống để có trải nghiệm hiện đại hơn)

---

## 🚀 Tính năng nổi bật (Key Features)

- ✅ **Product Management**: Quản lý sản phẩm đa biến thể, tích hợp tìm kiếm thông minh bằng **Vector Search**.
- ✅ **Shopping Cart & Orders**: Hệ thống giỏ hàng linh hoạt và quy trình đặt hàng chặt chẽ.
- ✅ **Secure Payments**: Tích hợp thanh toán trực tuyến an toàn qua **Stripe**.
- ✅ **Hybrid Caching**: Sử dụng hệ thống cache kết hợp (Memory + Redis) giúp tối ưu hiệu năng truy vấn.
- ✅ **Automated Background Jobs**: Tự động hóa các tác vụ như gửi mail, dọn dẹp dữ liệu, và đồng bộ hóa bằng **Hangfire**.
- ✅ **PDF Invoices**: Tự động xuất hóa đơn PDF chuyên nghiệp khi hoàn tất đơn hàng bằng **QuestPDF**.
- ✅ **Advanced Observability**: Theo dõi hiệu năng, lỗi và luồng xử lý ứng dụng thông qua **OpenTelemetry** và **Seq**.
- ✅ **Clean Code & Best Practices**: Áp dụng Global Usings, Error Handling Middleware, và Dependency Injection patterns.

---

## 📥 Hướng dẫn cài đặt (Installation)

### 📋 Yêu cầu hệ thống (Prerequisites)
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (Yêu cầu cài đặt thêm extension [pgvector](https://github.com/pgvector/pgvector))
- [Redis](https://redis.io/download/) (Optional nhưng khuyến nghị để tối ưu cache)
- [Docker](https://www.docker.com/) (Để chạy các service phụ trợ dễ dàng hơn)

### 🛠️ Các bước thực hiện
1.  **Clone mã nguồn**:
    ```bash
    git clone https://github.com/TrongNghia-17/TechShop.ECommerce.git
    cd TechShop.ECommerce
    ```
2.  **Cấu hình môi trường**:
    Cập nhật ConnectionStrings và các ApiKeys (Stripe, SendGrid, Azure) trong file `src/API/TechShop.ECommerce.Api/appsettings.Development.json`.
3.  **Khởi tạo Database**:
    ```bash
    dotnet ef database update --project src/Infrastructure/TechShop.ECommerce.Persistence --startup-project src/API/TechShop.ECommerce.Api
    ```
4.  **Chạy ứng dụng**:
    ```bash
    dotnet run --project src/API/TechShop.ECommerce.Api
    ```
5.  **Truy cập API Documentation**:
    Mở trình duyệt và truy cập `https://localhost:PORT/scalar/v1`

---

## 🤝 Đóng góp (Contributing)
Mọi đóng góp nhằm cải thiện hệ thống đều được hoan nghênh. Vui lòng tạo **Issue** hoặc gửi **Pull Request**.

## 📄 Giấy phép (License)
Dự án được phát hành dưới giấy phép **MIT**. Xem file [LICENSE](LICENSE) để biết thêm chi tiết.

---
⭐ Nếu bạn thấy dự án này hữu ích, hãy cho mình một **Star** trên GitHub nhé!
