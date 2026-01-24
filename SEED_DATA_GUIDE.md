# 🎯 HƯỚNG DẪN SEED DATA VÀO DATABASE

## ⚠️ QUAN TRỌNG: Dừng ứng dụng đang chạy trước

Nếu ứng dụng đang chạy, hãy:

1. Nhấn `Ctrl + C` trong terminal đang chạy
2. Hoặc đóng trình duyệt và đợi vài giây

## 📊 Dữ Liệu Mẫu Sẽ Được Tạo

### 👥 **8 Members (Users):**

| Email           | Password  | Tên đầy đủ       | Rank | Role   |
| --------------- | --------- | ---------------- | ---- | ------ |
| admin@pcm.vn    | Admin123  | Nguyễn Văn Admin | 5.5  | Admin  |
| ducminh@pcm.vn  | Member123 | Đức Minh         | 3.2  | Member |
| thuha@pcm.vn    | Member123 | Thu Hà           | 2.8  | Member |
| vanlong@pcm.vn  | Member123 | Văn Long         | 4.1  | Member |
| mylinh@pcm.vn   | Member123 | Mỹ Linh          | 3.5  | Member |
| hoangan@pcm.vn  | Member123 | Hoàng An         | 2.3  | Member |
| kimnga@pcm.vn   | Member123 | Kim Nga          | 3.9  | Member |
| quoctuan@pcm.vn | Member123 | Quốc Tuấn        | 4.5  | Member |

### 🔥 **4 Challenges (Kèo):**

1. **"Thách đấu cuối tuần - Ai dám nhận?"** (Open)
   - Người tạo: Đức Minh
   - Mô tả: Chấp nửa trái, đấu Singles, Thứ 7 9AM

2. **"Doubles 2vs2 - Tìm đối thủ mạnh"** (Accepted)
   - Người tạo: Thu Hà
   - Mô tả: Team rank 3.0+ tìm đối thủ xứng tầm

3. **"Challenge cho người mới - Friendly match"** (Open)
   - Người tạo: Văn Long
   - Mô tả: Giao hữu không tính điểm

4. **"Đấu tranh top 1 - Rank 4.0+"** (Completed)
   - Người tạo: Mỹ Linh
   - Mô tả: Chỉ nhận từ rank 4.0+, có giải thưởng

### 🏆 **3 Matches (Trận đấu đã diễn ra):**

1. **Singles** - Mỹ Linh thắng Văn Long (Tính điểm, từ Challenge)
2. **Doubles** - Kim Nga & Hoàng An thắng Đức Minh & Thu Hà (Tính điểm, từ Challenge)
3. **Singles** - Quốc Tuấn thắng Hoàng An (Không tính điểm, tự do)

### 📅 **4 Bookings (Đặt sân):**

1. Đức Minh - Ngày mai 9:00-11:00
2. Thu Hà - Ngày mai 14:00-16:00
3. Văn Long - Ngày kia 8:00-10:00
4. Mỹ Linh - Ngày kia 16:00-18:00

## 🚀 Cách Chạy Seed Data

### Bước 1: Dừng ứng dụng

```bash
# Nhấn Ctrl + C trong terminal đang chạy ứng dụng
```

### Bước 2: Xóa database cũ (nếu muốn reset)

```bash
# Mở SQL Server Management Studio hoặc Azure Data Studio
# Chạy lệnh:
DROP DATABASE PCM_396;
```

### Bước 3: Xóa migrations (nếu đã reset database)

```bash
# Xóa thư mục Migrations
rm -rf Migrations
# Hoặc trên Windows:
# rmdir /s /q Migrations
```

### Bước 4: Tạo migration mới (nếu đã xóa)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Bước 5: Chạy ứng dụng để seed data

```bash
dotnet run
```

## ✅ Kiểm Tra Seed Data Thành Công

Khi chạy `dotnet run`, bạn sẽ thấy log:

```
✅ Seed data completed successfully!
📊 Created: 8 members, 4 challenges, 3 matches, 4 bookings
```

## 🔐 Đăng Nhập Kiểm Tra

### Tài khoản Admin:

- **Email:** admin@pcm.vn
- **Password:** Admin123
- **Quyền:** Có thể nhập kết quả trận đấu, quản lý kèo

### Tài khoản Member (ví dụ):

- **Email:** ducminh@pcm.vn
- **Password:** Member123
- **Quyền:** Tạo kèo, nhận kèo, đặt sân, xem lịch sử

## 📝 Lưu Ý

1. ⚠️ Seed data chỉ chạy khi database trống (chưa có Members)
2. 🔄 Nếu muốn seed lại, xóa toàn bộ dữ liệu trong database
3. 💾 Dữ liệu seed chỉ chạy 1 lần khi khởi động ứng dụng
4. 🎯 Tất cả password đều là: **Admin123** (Admin) hoặc **Member123** (Members)

## 🎨 Kiểm Tra Các Tính Năng

Sau khi seed data, bạn có thể test:

### Với Admin (admin@pcm.vn):

- ✅ Nhập kết quả trận đấu mới
- ✅ Xem quản lý tất cả kèo (Open/Accepted/Completed)
- ✅ Tất cả chức năng của Member

### Với Member:

- ✅ Xem danh sách kèo đang mở
- ✅ Tạo kèo mới
- ✅ Nhận kèo của người khác
- ✅ Xem lịch sử trận đấu (có 3 trận mẫu)
- ✅ Đặt sân (có 4 booking mẫu)
- ✅ Tìm kiếm kèo

## 🔥 Demo Flow Hoàn Chỉnh

1. **Đăng nhập Admin** → Vào "Quản lý kèo" xem 4 kèo
2. **Vào "Nhập kết quả"** → Chọn challenge "Doubles 2vs2" đã Accepted
3. **Nhập kết quả** → Xem rank của winners tăng +0.1
4. **Đăng xuất** → Đăng nhập Member (ducminh@pcm.vn)
5. **Vào "Tìm Kèo"** → Thấy 2 kèo Open
6. **Nhận kèo** → Kèo chuyển sang Accepted
7. **Vào "Đặt sân"** → Thấy 4 bookings gần đây
8. **Đặt booking mới** → Hệ thống check trùng lịch
9. **Vào "Lịch sử đấu"** → Thấy 3 trận đã đấu với đầy đủ thông tin

---

💡 **Tip:** Database đã có sẵn dữ liệu phong phú để demo tất cả tính năng!
