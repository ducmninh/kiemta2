-- =============================================
-- Script tạo SEED DATA cho Database PCM_396
-- Sử dụng khi Update-Database không chạy được
-- =============================================

USE PCM_396;
GO

-- Seed Members (5 members mẫu)
INSERT INTO [396_Members] (IdentityUserId, FullName, Status, RankLevel)
VALUES 
    ('seed-user-1', N'Nguyễn Văn An (Admin)', 'Active', 5.0),
    ('seed-user-2', N'Trần Thị Bình', 'Active', 3.5),
    ('seed-user-3', N'Lê Văn Cường', 'Active', 4.2),
    ('seed-user-4', N'Phạm Thị Dung', 'Active', 2.8),
    ('seed-user-5', N'Hoàng Văn Em', 'Active', 3.9);
GO

-- Seed Challenges (3 challenges với các status khác nhau)
INSERT INTO [396_Challenges] (CreatorId, Title, Description, Status, CreatedDate)
VALUES 
    (1, N'Thách đấu buổi sáng thứ 7', N'Tìm đối thủ xứng tầm cho trận đấu sáng thứ 7 tuần này', 0, DATEADD(DAY, -2, GETDATE())),
    (2, N'Kèo đôi chiều chủ nhật', N'Cần 1 cặp đôi chơi ranked match', 1, DATEADD(DAY, -1, GETDATE())),
    (3, N'Giao hữu tối thứ 6', N'Chơi vui vẻ không tính điểm', 2, DATEADD(DAY, -5, GETDATE()));
GO

-- Seed Matches (2 matches - 1 đơn, 1 đôi)
INSERT INTO [396_Matches] (MatchDate, Format, IsRanked, ChallengeId, Winner1Id, Winner2Id, Loser1Id, Loser2Id)
VALUES 
    (DATEADD(DAY, -3, GETDATE()), 0, 1, 3, 1, NULL, 3, NULL),
    (DATEADD(DAY, -1, GETDATE()), 1, 1, NULL, 2, 4, 3, 5);
GO

-- Seed Bookings (2 bookings)
INSERT INTO [396_Bookings] (MemberId, StartTime, EndTime, CreatedDate)
VALUES 
    (1, DATEADD(HOUR, 8, DATEADD(DAY, 1, CAST(GETDATE() AS DATE))), DATEADD(HOUR, 10, DATEADD(DAY, 1, CAST(GETDATE() AS DATE))), GETDATE()),
    (2, DATEADD(HOUR, 14, DATEADD(DAY, 2, CAST(GETDATE() AS DATE))), DATEADD(HOUR, 16, DATEADD(DAY, 2, CAST(GETDATE() AS DATE))), GETDATE());
GO

PRINT 'Seed data inserted successfully!'
PRINT '- 5 Members'
PRINT '- 3 Challenges (Open, Accepted, Completed)'
PRINT '- 2 Matches (1 Singles, 1 Doubles)'  
PRINT '- 2 Bookings'
GO
