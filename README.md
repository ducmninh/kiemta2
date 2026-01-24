# PCM_396 - Hệ thống Quản lý CLB Pickleball "Vợt Thủ Phô Nữi"

## 🎯 HƯỚNG DẪN NHANH CHO GIẢNG VIÊN

### Các bước chạy project:

1. **Clone repository**

   ```bash
   git clone <repository-url>
   cd kiemtra2
   ```

2. **Cấu hình Connection String** trong `appsettings.json`

   Mở file [appsettings.json](appsettings.json) và sửa Connection String theo SQL Server của bạn:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=TEN_MAY_CHU\\TEN_INSTANCE;Database=PCM_396;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
   }
   ```

   **Ví dụ:**
   - Server local: `Server=localhost;Database=PCM_396;...`
   - SQL Express: `Server=.\\SQLEXPRESS;Database=PCM_396;...`
   - SQL Server tùy chỉnh: `Server=DUCMINH\\SQLEXPRESS01;Database=PCM_396;...`

3. **Cài đặt packages (nếu cần):**

   ```bash
   dotnet restore
   ```

4. **Tạo database và chạy Migration:**

   **Cách 1: Dùng Package Manager Console trong Visual Studio**

   ```powershell
   Update-Database
   ```

   **Cách 2: Dùng .NET CLI (Command Line)**

   ```bash
   dotnet ef database update
   ```

   **Lưu ý:** Nếu báo lỗi `dotnet ef not found`, cài đặt EF Core tools:

   ```bash
   dotnet tool install --global dotnet-ef
   ```

5. **Chạy ứng dụng:**

   **Cách 1: Visual Studio**
   - Mở file `PCM_396.csproj` hoặc `kiemtra2.sln`
   - Nhấn F5 hoặc nút ▶️ "Run"

   **Cách 2: Command Line**

   ```bash
   dotnet run
   ```

   hoặc chỉ định project:

   ```bash
   dotnet run --project PCM_396.csproj
   ```

6. **Truy cập ứng dụng:**

   Mở trình duyệt và truy cập:
   - **HTTP**: http://localhost:5000
   - **HTTPS**: https://localhost:5001

   Nếu cổng khác, xem thông báo trên terminal sau khi chạy `dotnet run`

7. **✅ TỰ ĐỘNG TẠO ADMIN & SEED DATA** khi app chạy lần đầu:

   **Tài khoản Admin:**
   - Email: `admin@pcm.vn`
   - Password: `Admin123!`
   - Role: Administrator (có thể ghi nhận kết quả trận đấu)

   **Seed Data (từ Migration):**
   - 5 Members mẫu
   - 3 Challenges (Open, Accepted, Completed)
   - 2 Matches (Singles & Doubles)
   - 2 Bookings

   **Seed Data bổ sung (từ Program.cs):**
   - 8 Members với tài khoản đăng nhập
   - 4 Challenges đa dạng
   - 3 Matches với kết quả khác nhau
   - 4 Bookings trong các khung giờ khác nhau

8. **Đăng nhập và kiểm tra:**

   Sử dụng tài khoản Admin để test đầy đủ chức năng:
   - 📧 Email: `admin@pcm.vn`
   - 🔑 Password: `Admin123!`

   Hoặc đăng ký tài khoản mới tại: http://localhost:5000/Identity/Account/Register

9. **Phương án dự phòng**: Nếu seed data không tự động, chạy file **`SeedData.sql`** bằng SQL Server Management Studio

### Files quan trọng:

- ✅ `README.md` - File này (hướng dẫn đầy đủ)
- ✅ `CHECKLIST_GIAOVIEN.md` - **Checklist kiểm tra dành cho giảng viên**
- ✅ `TROUBLESHOOTING.md` - **Giải quyết các vấn đề thường gặp**
- ✅ `Migrations/` - Folder chứa migration files với seed data
- ✅ `SeedData.sql` - **Script SQL dự phòng cho seed data** (quan trọng!)
- ✅ `Data/ApplicationDbContext.cs` - Có method SeedData() với đầy đủ dữ liệu mẫu

---

## Thông tin dự án

- **Môn học**: Lập trình Backend với ASP.NET Core
- **MSSV**: xxx396
- **Công nghệ**: ASP.NET Core 8.0 Razor Pages
- **Database**: SQL Server (Entity Framework Core - Code First)

## Chức năng chính

### 1. Xác thực & Giao diện (Câu 1)

- ✅ Đăng ký/Đăng nhập với Identity
- ✅ Hỗ trợ Google Sign-in
- ✅ Phân quyền: Admin (Referee) & Member
- ✅ Layout động theo role

### 2. Sàn đấu Kèo (Câu 2)

- ✅ Hiển thị các kèo đang Open
- ✅ Tạo kèo mới
- ✅ Nhận kèo (Accept) → chuyển status sang Accepted
- ✅ **BONUS**: Tìm kiếm kèo theo keyword

### 3. Ghi nhận kết quả (Câu 3)

- ✅ Chỉ Admin/Referee được nhập kết quả
- ✅ Chọn Format: Đơn (1vs1) hoặc Đôi (2vs2)
- ✅ Chọn Challenge từ danh sách Accepted hoặc đánh tự do
- ✅ Chọn người thắng/thua
- ✅ Checkbox IsRanked: Tính điểm (+0.1 winner, -0.1 loser, min 1.0)
- ✅ Tự động cập nhật status Challenge thành Completed
- ✅ **BONUS**: Hiển thị kết quả đẹp mắt với icon Challenge Match

### 4. Đặt sân (Câu 4)

- ✅ Đặt lịch chơi với StartTime và EndTime
- ✅ Validation: StartTime < EndTime
- ✅ Logic kiểm tra Overlap: Không cho đặt trùng lịch

## Kiến trúc dự án

### Models (Database Tables - Prefix: 396\_)

- `396_Members`: Hội viên (Id, IdentityUserId, FullName, Status, RankLevel)
- `396_Challenges`: Kèo đấu (Id, CreatorId, Title, Description, Status)
- `396_Matches`: Trận đấu (Id, Format, IsRanked, ChallengeId, Winner1/2Id, Loser1/2Id)
- `396_Bookings`: Đặt sân (Id, MemberId, StartTime, EndTime)

### Services Layer

- `IMemberService` / `MemberService`: Quản lý hội viên
- `IChallengeService` / `ChallengeService`: Quản lý kèo đấu
- `IMatchService` / `MatchService`: Quản lý trận đấu & tính điểm
- `IBookingService` / `BookingService`: Quản lý đặt sân & kiểm tra overlap

### Razor Pages

- `/Index`: Trang chủ
- `/Challenges/Index`: Sàn đấu kèo
- `/Challenges/Create`: Tạo kèo mới
- `/Matches/Create`: Nhập kết quả (Admin only)
- `/Matches/History`: Lịch sử trận đấu
- `/Bookings/Create`: Đặt sân

## Cài đặt & Chạy dự án

### Yêu cầu

- .NET 8.0 SDK trở lên
- SQL Server (hoặc SQL Server Express/LocalDB)
- Visual Studio 2022 hoặc VS Code + C# Extension

### Các bước chạy (QUAN TRỌNG - Dành cho giảng viên)

#### Bước 1: Clone repository

```bash
git clone <repository-url>
cd kiemtra2
```

#### Bước 2: Cấu hình Connection String

Mở file `appsettings.json` và cập nhật connection string phù hợp với SQL Server của bạn:

**Ví dụ với SQL Server:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PCM_396_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

**Ví dụ với LocalDB (mặc định):**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PCM_396_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

#### Bước 3: Chạy Migration & Seed Data

**Sử dụng Package Manager Console (Visual Studio):**

```powershell
Update-Database
```

**Hoặc sử dụng .NET CLI:**

```bash
dotnet ef database update
```

✅ **Lưu ý**: Migration đã bao gồm **SEEDING DATA** với:

- 5 Members mẫu
- 3 Challenges (Open, Accepted, Completed)
- 2 Matches (1 đơn, 1 đôi)
- 2 Bookings

Sau khi chạy `Update-Database`, database sẽ được tạo tự động với đầy đủ dữ liệu mẫu.

**Phương án dự phòng - Sử dụng SQL Script:**

Nếu `Update-Database` gặp lỗi hoặc không seed data, sử dụng file `SeedData.sql`:

1. Mở SQL Server Management Studio
2. Connect tới SQL Server của bạn
3. Mở file `SeedData.sql` trong project
4. Thực thi script (F5) để insert seed data

Script sẽ tạo đầy đủ dữ liệu mẫu như trên.

#### Bước 4: Chạy ứng dụng

```bash
dotnet run
```

Hoặc nhấn **F5** trong Visual Studio.

Ứng dụng sẽ chạy tại: `https://localhost:5001` hoặc `http://localhost:5000`

### Thông tin đăng nhập

Sau khi chạy ứng dụng, bạn cần **đăng ký tài khoản mới** để sử dụng.

**Để trở thành Admin (Referee):**

1. Đăng ký tài khoản mới
2. Sử dụng SQL Server Management Studio hoặc query trực tiếp để thêm role:

```sql
-- Thêm role Admin/Referee vào AspNetRoles (nếu chưa có)
INSERT INTO AspNetRoles (Id, Name, NormalizedName)
VALUES (NEWID(), 'Referee', 'REFEREE')

-- Gán role cho user (thay YOUR_USER_ID bằng Id từ AspNetUsers)
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT 'YOUR_USER_ID', Id FROM AspNetRoles WHERE Name = 'Referee'
```

5. **Truy cập ứng dụng**
   - Mở browser: `https://localhost:5001` hoặc `http://localhost:5000`

## Tài khoản mặc định

### Tạo Admin (Referee)

1. Đăng ký tài khoản mới qua UI
2. Vào SQL Server, chạy query:

```sql
-- Lấy UserId của user vừa tạo
SELECT Id, UserName FROM AspNetUsers;

-- Lấy RoleId của Admin
SELECT Id FROM AspNetRoles WHERE Name = 'Admin';

-- Gán role Admin cho user
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('user-id-here', 'admin-role-id-here');
```

## Logic nghiệp vụ quan trọng

### 1. Challenge Status Flow

```
Open (0) → Accepted (1) → Completed (2)
```

### 2. Match Creation Logic

- Nếu có `ChallengeId` → Cập nhật Challenge thành Completed
- Nếu `IsRanked = true`:
  - Winner(s): RankLevel += 0.1
  - Loser(s): RankLevel = Math.Max(1.0, RankLevel - 0.1)

### 3. Booking Overlap Check

```csharp
// Hai khoảng thời gian (A, B) và (C, D) trùng nhau khi:
// A < D && B > C
bool hasOverlap = existingBooking.StartTime < newBooking.EndTime &&
                  existingBooking.EndTime > newBooking.StartTime;
```

## Ghi chú quan trọng

- ⚠️ **Google Sign-in**: Cần cấu hình ClientId và ClientSecret trong `appsettings.json`
- ⚠️ Tất cả bảng database phải có prefix **396\_** (3 số cuối MSSV)
- ⚠️ Logic tính điểm và xử lý Challenge status được implement trong **Service Layer**
- ⚠️ Chỉ Admin/Referee mới được nhập kết quả trận đấu

## Author

- **MSSV**: xxx396
- **Email**: [your-email]
- **GitHub**: [your-github-repo]

---

**Chúc mừng! Dự án đã hoàn thiện theo đúng yêu cầu đề bài.** 🎉
