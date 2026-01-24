# ✅ CHECKLIST KIỂM TRA - PCM_396

## Yêu cầu từ giảng viên:

### 1. ✅ Repository có README.md mô tả cách chạy project

- **File**: `README.md`
- **Nội dung**: Hướng dẫn chi tiết từng bước clone, config, migration, và chạy project
- **Vị trí**: Root folder của repository

### 2. ✅ Có file migration hoặc script tạo database

- **Migrations folder**: `Migrations/` - Chứa các file migration
- **Migration files**:
  - `CreateDatabaseWithSeedData.cs` - Migration chính tạo database
  - `ApplicationDbContextModelSnapshot.cs` - Snapshot của database schema
- **Script SQL dự phòng**: `SeedData.sql` - Có thể chạy trực tiếp trong SSMS

### 3. ✅ Seeding Data cho kiểm tra

- **Trong Code**: `Data/ApplicationDbContext.cs` - Method `SeedData()` có:
  - 5 Members mẫu (Id 1-5)
  - 3 Challenges với các status: Open, Accepted, Completed
  - 2 Matches: 1 trận Đơn (Singles) và 1 trận Đôi (Doubles)
  - 2 Bookings với thời gian khác nhau

- **Trong SQL Script**: `SeedData.sql` - Insert statements đầy đủ

## Các bước kiểm tra:

### Bước 1: Clone repository

```bash
git clone <repository-url>
cd kiemtra2
```

### Bước 2: Config Connection String

Mở `appsettings.json`, sửa:

```json
"DefaultConnection": "Server=YOUR_SERVER;Database=PCM_396_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### Bước 3: Chạy Migration

**Visual Studio Package Manager Console:**

```powershell
Update-Database
```

**Hoặc .NET CLI:**

```bash
dotnet ef database update
```

### Bước 4: Kiểm tra Seed Data

Sau khi chạy Update-Database, mở SQL Server Management Studio và kiểm tra:

```sql
USE PCM_396_DB;

-- Kiểm tra Members (phải có 5 records)
SELECT * FROM [396_Members];

-- Kiểm tra Challenges (phải có 3 records với status khác nhau)
SELECT * FROM [396_Challenges];

-- Kiểm tra Matches (phải có 2 records - 1 Singles, 1 Doubles)
SELECT * FROM [396_Matches];

-- Kiểm tra Bookings (phải có 2 records)
SELECT * FROM [396_Bookings];
```

### Bước 5: Chạy ứng dụng

```bash
dotnet run
```

Hoặc nhấn F5 trong Visual Studio.

## Kết quả mong đợi:

✅ Database được tạo tự động  
✅ 5 bảng chính: `396_Members`, `396_Challenges`, `396_Matches`, `396_Bookings`, và các bảng Identity  
✅ Dữ liệu mẫu đã được insert tự động  
✅ Ứng dụng chạy được tại https://localhost:5001

## Nếu gặp vấn đề:

### Vấn đề: Seed data không tự động insert

**Giải pháp**: Chạy file `SeedData.sql` bằng SSMS:

1. Mở SQL Server Management Studio
2. Connect tới SQL Server
3. File → Open → File → Chọn `SeedData.sql`
4. Nhấn F5 để execute

### Vấn đề: Migration không chạy được

**Giải pháp 1**: Xóa database và chạy lại

```powershell
Drop-Database
Update-Database
```

**Giải pháp 2**: Tạo database thủ công và chạy migration

```sql
CREATE DATABASE PCM_396_DB;
```

Sau đó chạy `Update-Database`

## Liên hệ

- **MSSV**: xxx396
- **Môn học**: Lập trình Backend với ASP.NET Core
