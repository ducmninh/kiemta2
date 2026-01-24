# 📦 DANH SÁCH FILES QUAN TRỌNG

## 📄 Files Documentation (Đọc theo thứ tự)

1. **SUBMISSION_GUIDE.md** ⭐⭐⭐  
   → Bắt đầu từ đây! Hướng dẫn tổng quan cho giảng viên

2. **README.md** ⭐⭐⭐  
   → Hướng dẫn chi tiết cách cài đặt và chạy project

3. **CHECKLIST_GIAOVIEN.md** ⭐⭐  
   → Checklist kiểm tra từng bước một

4. **TROUBLESHOOTING.md** ⭐  
   → Giải quyết các vấn đề thường gặp

5. **IMPORTANT_FILES.md** (File này)  
   → Danh sách tất cả files quan trọng

## 🗄️ Files Database & Migration

1. **SeedData.sql** ⭐⭐⭐ **[QUAN TRỌNG NHẤT]**  
   → Script SQL tạo dữ liệu mẫu (dự phòng)  
   → Sử dụng khi Update-Database không seed data tự động

2. **Migrations/** folder  
   → Chứa các file migration để tạo database schema

3. **Data/ApplicationDbContext.cs**  
   → Có method `SeedData()` với dữ liệu mẫu được định nghĩa trong code

## ⚙️ Files Configuration

1. **appsettings.json**  
   → **CẦN SỬA** Connection String trước khi chạy

2. **PCM_396.csproj**  
   → Project file chính

3. **Program.cs**  
   → Entry point của ứng dụng

## 📝 Seed Data Details

Khi chạy `Update-Database` hoặc execute `SeedData.sql`, sẽ tạo:

### Members (5 records)

- Nguyễn Văn An (Admin) - RankLevel: 5.0
- Trần Thị Bình - RankLevel: 3.5
- Lê Văn Cường - RankLevel: 4.2
- Phạm Thị Dung - RankLevel: 2.8
- Hoàng Văn Em - RankLevel: 3.9

### Challenges (3 records)

- "Thách đấu buổi sáng thứ 7" - Status: Open (0)
- "Kèo đôi chiều chủ nhật" - Status: Accepted (1)
- "Giao hữu tối thứ 6" - Status: Completed (2)

### Matches (2 records)

- Match 1: Singles (Format=0), Ranked, Challenge #3
  - Winner: Member #1 (Nguyễn Văn An)
  - Loser: Member #3 (Lê Văn Cường)

- Match 2: Doubles (Format=1), Ranked, Free match
  - Winners: Member #2 + #4 (Trần Thị Bình + Phạm Thị Dung)
  - Losers: Member #3 + #5 (Lê Văn Cường + Hoàng Văn Em)

### Bookings (2 records)

- Booking 1: Member #1, Tomorrow 8AM-10AM
- Booking 2: Member #2, Day after tomorrow 2PM-4PM

## 🔍 Cách kiểm tra nhanh

```sql
USE PCM_396_DB;

-- Kiểm tra tất cả trong 1 query
SELECT
    (SELECT COUNT(*) FROM [396_Members]) AS Members,
    (SELECT COUNT(*) FROM [396_Challenges]) AS Challenges,
    (SELECT COUNT(*) FROM [396_Matches]) AS Matches,
    (SELECT COUNT(*) FROM [396_Bookings]) AS Bookings;
```

**Kết quả mong đợi:**

```
Members | Challenges | Matches | Bookings
   5    |     3      |    2    |    2
```

## ✅ Quick Start

```bash
# 1. Clone repo
git clone <url>
cd kiemtra2

# 2. Sửa appsettings.json (Connection String)

# 3. Run migration
dotnet ef database update

# 4. Kiểm tra data bằng SQL query ở trên

# 5. Chạy app
dotnet run
```

## 📞 Nếu cần hỗ trợ

1. Đọc `SUBMISSION_GUIDE.md`
2. Đọc `TROUBLESHOOTING.md`
3. Chạy `SeedData.sql` nếu cần

---

**Tất cả files đều được thiết kế để giảng viên có thể:**

- ✅ Clone repository
- ✅ Chạy Update-Database
- ✅ Kiểm tra seed data
- ✅ Chấm điểm dễ dàng

**Không có lý do nào để bị 0 điểm!** 💯
