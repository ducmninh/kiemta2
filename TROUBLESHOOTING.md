# 🔧 TROUBLESHOOTING - Giải quyết các vấn đề thường gặp

## 1. Migration không chạy được / Build failed

### Triệu chứng:

```
Build started...
Build failed. Use dotnet build to see the errors.
```

### Nguyên nhân:

- Process đang chạy
- Conflict giữa các file

### Giải pháp:

**Bước 1**: Dừng tất cả process đang chạy

- Đóng tất cả terminal/console
- Trong Task Manager, tìm và End Task `PCM_396.exe`

**Bước 2**: Clean và rebuild

```bash
dotnet clean PCM_396.csproj
dotnet build PCM_396.csproj
```

**Bước 3**: Chạy migration lại

```bash
dotnet ef database update
```

---

## 2. Seed Data không được tạo tự động

### Triệu chứng:

- Migration chạy thành công
- Database được tạo nhưng các bảng trống

### Giải pháp:

**Phương án 1**: Sử dụng SQL Script

1. Mở SQL Server Management Studio (SSMS)
2. Connect tới SQL Server
3. File → Open → File
4. Chọn file `SeedData.sql` trong project root
5. Nhấn F5 để execute script
6. Kiểm tra kết quả:

```sql
SELECT COUNT(*) FROM [396_Members];   -- Phải là 5
SELECT COUNT(*) FROM [396_Challenges]; -- Phải là 3
SELECT COUNT(*) FROM [396_Matches];    -- Phải là 2
SELECT COUNT(*) FROM [396_Bookings];   -- Phải là 2
```

**Phương án 2**: Xóa database và tạo lại

```powershell
# Trong Package Manager Console
Drop-Database -Force
Update-Database
```

---

## 3. Connection String không đúng

### Triệu chứng:

```
SqlException: A network-related or instance-specific error occurred...
```

### Giải pháp:

Mở `appsettings.json` và sửa Connection String:

**Với SQL Server:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PCM_396_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

**Với LocalDB:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PCM_396_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

**Với SQL Server Express:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PCM_396_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

**Cách tìm tên SQL Server của bạn:**

1. Mở SQL Server Management Studio
2. Xem tên Server khi connect
3. Copy tên đó vào Connection String

---

## 4. Migration files không có trong folder Migrations

### Giải pháp:

**Tạo migration mới:**

```bash
# Xóa database cũ (nếu có)
dotnet ef database drop --force

# Xóa migration cũ (nếu có)
rm -rf Migrations

# Tạo migration mới
dotnet ef migrations add InitialCreate

# Apply migration
dotnet ef database update
```

---

## 5. Entity Framework tools không được cài đặt

### Triệu chứng:

```
No executable found matching command "dotnet-ef"
```

### Giải pháp:

```bash
dotnet tool install --global dotnet-ef
```

Sau đó chạy lại migration command.

---

## 6. Port đã được sử dụng

### Triệu chứng:

```
Failed to bind to address https://localhost:5001
```

### Giải pháp:

**Phương án 1**: Thay đổi port trong `launchSettings.json`

```json
"applicationUrl": "https://localhost:5002;http://localhost:5003"
```

**Phương án 2**: Kill process đang dùng port

```bash
# Windows
netstat -ano | findstr :5001
taskkill /PID <process_id> /F
```

---

## 7. Kiểm tra nhanh mọi thứ đã OK

### Checklist:

```bash
# 1. Kiểm tra .NET SDK
dotnet --version
# Kết quả mong đợi: 8.0.x hoặc cao hơn

# 2. Kiểm tra EF Tools
dotnet ef --version
# Kết quả mong đợi: 8.0.x hoặc cao hơn

# 3. Kiểm tra SQL Server
# Mở SSMS và connect thành công

# 4. Build project
dotnet build PCM_396.csproj
# Kết quả mong đợi: Build succeeded

# 5. Kiểm tra migration
dotnet ef migrations list
# Phải thấy ít nhất 1 migration

# 6. Apply migration
dotnet ef database update
# Kết quả mong đợi: Done

# 7. Chạy app
dotnet run
# Kết quả mong đợi: Now listening on: https://localhost:5001
```

---

## Liên hệ hỗ trợ

Nếu vẫn gặp vấn đề sau khi thử các giải pháp trên:

1. Kiểm tra lại từng bước trong `README.md`
2. Xem file `CHECKLIST_GIAOVIEN.md` để đảm bảo không bỏ sót bước nào
3. Chạy script `SeedData.sql` để đảm bảo có dữ liệu mẫu
