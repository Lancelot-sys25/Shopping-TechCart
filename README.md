# shoppingTechCart

Ứng dụng quản lý và mua sắm sản phẩm công nghệ được xây dựng bằng WPF trên .NET 8. Project sử dụng Entity Framework Core để làm việc với SQL Server.

## Công nghệ sử dụng

- C# / .NET 8
- WPF
- Entity Framework Core 8
- SQL Server
- Microsoft.Extensions.Configuration.Json

## Chức năng chính

### Đăng nhập và đăng ký

- Đăng nhập bằng tài khoản và mật khẩu.
- Chỉ cho phép đăng nhập với tài khoản đang hoạt động (`IsUse = true`).
- Phân quyền theo `RoleInSystem`:
  - `1`: Admin, mở màn hình quản lý sản phẩm.
  - `0`: User, mở màn hình mua hàng.
- Đăng ký tài khoản user mới.
- Kiểm tra dữ liệu đầu vào khi đăng ký như tài khoản, mật khẩu, họ tên và số điện thoại.

### Admin

- Xem danh sách sản phẩm.
- Thêm sản phẩm.
- Cập nhật thông tin sản phẩm.
- Xóa sản phẩm.
- Chọn danh mục sản phẩm.

### User

- Xem danh sách sản phẩm.
- Thêm sản phẩm vào giỏ hàng.
- Xóa sản phẩm khỏi giỏ hàng.
- Tạo đơn hàng từ giỏ hàng.
- Thanh toán đơn hàng bằng tiền mặt.

## Cấu trúc thư mục

```text
shoppingTechCart/
├── App.xaml
├── LoginWindow.xaml
├── RegisterWindow.xaml
├── MainWindow.xaml
├── UserWindow.xaml
├── appsettings.example.json
├── shoppingTechCart.csproj
└── Entities/
    ├── Account.cs
    ├── CartItem.cs
    ├── Category.cs
    ├── Product.cs
    ├── ProductIntroContext.cs
    ├── Order.cs
    ├── OrderDetail.cs
    ├── SessionToken.cs
    └── ViewHistory.cs
```

## Cơ sở dữ liệu

Project đang kết nối tới database SQL Server tên `ProductIntro`. Các bảng được ánh xạ trong `ProductIntroContext` gồm:

- `accounts`
- `products`
- `categories`
- `cart_items`
- `orders`
- `order_details`
- `session_tokens`
- `view_history`

## Cấu hình kết nối database

Trong thư mục `shoppingTechCart`, tạo file `appsettings.json` dựa trên file mẫu `appsettings.example.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;User Id=YOUR_USER;Password=YOUR_PASSWORD;Database=ProductIntro;TrustServerCertificate=True;"
  }
}
```

Thay các giá trị sau bằng thông tin SQL Server của bạn:

- `YOUR_SERVER`: tên server SQL Server
- `YOUR_USER`: user đăng nhập SQL Server
- `YOUR_PASSWORD`: mật khẩu SQL Server

Ví dụ:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;User Id=sa;Password=your_password;Database=ProductIntro;TrustServerCertificate=True;"
  }
}
```

## Cách chạy project

1. Cài đặt .NET 8 SDK.
2. Cài đặt và mở SQL Server.
3. Tạo database `ProductIntro` và các bảng cần thiết.
4. Tạo file `shoppingTechCart/appsettings.json` từ file mẫu.
5. Mở solution bằng Visual Studio hoặc chạy bằng command line:

```bash
dotnet restore
dotnet build
dotnet run --project shoppingTechCart/shoppingTechCart.csproj
```

Khi chạy ứng dụng, màn hình đầu tiên là `LoginWindow`.

## Ghi chú

- File `appsettings.json` chứa thông tin kết nối database thật nên không nên commit lên Git.
- File `appsettings.example.json` được dùng làm mẫu cấu hình cho người khác setup project.
- Tài khoản admin cần có `RoleInSystem = 1`.
- Tài khoản user thông thường có `RoleInSystem = 0`.
- Sản phẩm cần có `ProductId`, `ProductName`, `Price`, `Discount`, `Unit`, `TypeId` và `Account`.

