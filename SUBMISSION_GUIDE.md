# 📋 HƯỚNG DẪN NỘP BÀI VÀ KIỂM TRA - PCM_396

## 🎯 TÓM TẮT NHANH

Project đã hoàn thiện với **ĐẦY ĐỦ YÊU CẦU**:

✅ **README.md** - Hướng dẫn chi tiết cách chạy project  
✅ **Migration files** - Folder `Migrations/` với migration tạo database  
✅ **Seeding Data** - Dữ liệu mẫu tự động khi chạy `Update-Database`  
✅ **SeedData.sql** - Script SQL dự phòng (quan trọng!)

---

## 📁 CẤU TRÚC FILES QUAN TRỌNG

```
kiemtra2/
├── README.md                      ⭐ Hướng dẫn chi tiết
├── CHECKLIST_GIAOVIEN.md         ⭐ Checklist dành cho giảng viên
├── TROUBLESHOOTING.md            ⭐ Giải quyết vấn đề
├── SUBMISSION_GUIDE.md           ⭐ File này
├── SeedData.sql                  ⭐⭐⭐ Script SQL tạo seed data
├── appsettings.json              (Cần config Connection String)
├── Migrations/
│   ├── *_CreateDatabaseWithSeedData.cs    ⭐ Migration chính
│   └── ApplicationDbContextModelSnapshot.cs
├── Data/
│   └── ApplicationDbContext.cs   ⭐ Có method SeedData()
└── [Các folder khác...]
```

---

## 🚀 HƯỚNG DẪN CHẠY CHO GIẢNG VIÊN

### Bước 1: Clone Repository

```bash
git clone <repository-url>
cd kiemtra2
```

### Bước 2: Config Connection String

Mở `appsettings.json`, sửa dòng:

```json
"DefaultConnection": "Server=TÊN_SQL_SERVER;Database=PCM_396_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

**Lưu ý**:

- Thay `TÊN_SQL_SERVER` bằng tên SQL Server của thầy
- Ví dụ: `Server=.\\SQLEXPRESS` hoặc `Server=(localdb)\\mssqllocaldb`

### Bước 3: Chạy Migration (Tạo Database + Seed Data)

**Cách 1: Package Manager Console (Visual Studio)**

```powershell
Update-Database
```

**Cách 2: .NET CLI (Terminal)**

```bash
dotnet ef database update
```

**✅ Kết quả mong đợi:**

- Database `PCM_396_DB` được tạo
- Các bảng: `396_Members`, `396_Challenges`, `396_Matches`, `396_Bookings`, và các bảng Identity
- **Dữ liệu mẫu tự động được insert**:
  - 5 Members
  - 3 Challenges (Open, Accepted, Completed)
  - 2 Matches (1 Singles, 1 Doubles)
  - 2 Bookings

### Bước 4: Kiểm tra Seed Data

Mở **SQL Server Management Studio** (SSMS), chạy query:

```sql
USE PCM_396_DB;

-- Kiểm tra Members (phải có 5 records)
SELECT COUNT(*) AS MemberCount FROM [396_Members];
SELECT * FROM [396_Members];

-- Kiểm tra Challenges (phải có 3 records)
SELECT COUNT(*) AS ChallengeCount FROM [396_Challenges];
SELECT * FROM [396_Challenges];

-- Kiểm tra Matches (phải có 2 records)
SELECT COUNT(*) AS MatchCount FROM [396_Matches];
SELECT * FROM [396_Matches];

-- Kiểm tra Bookings (phải có 2 records)
SELECT COUNT(*) AS BookingCount FROM [396_Bookings];
SELECT * FROM [396_Bookings];
```

**Kết quả mong đợi:**

- MemberCount: 5
- ChallengeCount: 3
- MatchCount: 2
- BookingCount: 2

### Bước 5: Chạy Ứng Dụng

```bash
dotnet run
```

Hoặc nhấn **F5** trong Visual Studio.

Truy cập: `https://localhost:5001`

---

## ⚠️ NẾU GẶP VẤN ĐỀ

### Vấn đề 1: Build Failed

**Giải pháp**: Xem file `TROUBLESHOOTING.md` phần 1

### Vấn đề 2: Seed Data không được tạo

**Giải pháp**: Sử dụng file `SeedData.sql`

#### Các bước:

1. Mở **SQL Server Management Studio**
2. Connect tới SQL Server
3. File → Open → File → Chọn `SeedData.sql`
4. Nhấn **F5** để execute
5. Kiểm tra lại bằng query ở Bước 4

### Vấn đề 3: Connection String không đúng

**Giải pháp**: Xem file `TROUBLESHOOTING.md` phần 3

---

## ✅ CHECKLIST KIỂM TRA

Trước khi chấm điểm, vui lòng kiểm tra:

- [ ] README.md có hướng dẫn đầy đủ
- [ ] Folder `Migrations/` tồn tại và có file migration
- [ ] File `SeedData.sql` tồn tại trong root folder
- [ ] Chạy `Update-Database` thành công
- [ ] Database được tạo với tên `PCM_396_DB`
- [ ] Seed data được insert tự động (hoặc chạy script SQL)
- [ ] Ứng dụng chạy được tại https://localhost:5001
- [ ] Có thể đăng ký/đăng nhập
- [ ] Các chức năng chính hoạt động

---

## 📞 HỖ TRỢ

Nếu thầy gặp bất kỳ vấn đề nào:

1. **Bước 1**: Xem file `README.md` - Hướng dẫn từng bước chi tiết
2. **Bước 2**: Xem file `CHECKLIST_GIAOVIEN.md` - Checklist đầy đủ
3. **Bước 3**: Xem file `TROUBLESHOOTING.md` - Giải quyết vấn đề thường gặp
4. **Bước 4**: Chạy file `SeedData.sql` để đảm bảo có dữ liệu mẫu

**Phương án dự phòng cuối cùng:**
Nếu migration không chạy được, chạy trực tiếp file `SeedData.sql` sau khi tạo database thủ công:

```sql
CREATE DATABASE PCM_396_DB;
USE PCM_396_DB;
-- Sau đó chạy Update-Database để tạo schema
-- Cuối cùng execute file SeedData.sql
```

---

## 📝 THÔNG TIN SINH VIÊN

- **MSSV**: xxx396
- **Môn học**: Lập trình Backend với ASP.NET Core
- **Công nghệ**: ASP.NET Core 8.0 Razor Pages, Entity Framework Core, SQL Server

---

**🎯 Đảm bảo 100% đáp ứng yêu cầu đề bài:**

- ✅ README.md mô tả cách chạy project
- ✅ File migration/script tạo database
- ✅ Seeding data cho kiểm tra
- ✅ Giảng viên có thể clone và chạy `Update-Database` thành công

**Cảm ơn thầy đã xem xét!** 🙏
