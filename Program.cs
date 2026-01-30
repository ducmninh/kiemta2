using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Đăng ký DbContext với In-Memory Database (không cần SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("PCM_InMemoryDb"));

// Đăng ký Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Cấu hình Google Authentication
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
    });

// Đăng ký Services
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IChallengeService, ChallengeService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();

// Seed Roles và Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Tạo roles
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("Member"))
        {
            await roleManager.CreateAsync(new IdentityRole("Member"));
        }

        // Tạo tài khoản Admin mặc định nếu chưa có
        var adminEmail = "admin@pcm.vn";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                
                // Tạo Member record cho admin
                var adminMember = new PCM_396.Models.Member
                {
                    IdentityUserId = adminUser.Id,
                    FullName = "Administrator",
                    Status = "Active",
                    RankLevel = 5.0
                };
                context.Members.Add(adminMember);
                await context.SaveChangesAsync();

                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("✅ Admin account created successfully!");
                logger.LogInformation("📧 Email: {Email} | Password: Admin123!", adminEmail);
            }
        }

        // Seed dữ liệu mẫu nếu chưa có (không tính admin)
        if (context.Members.Count() <= 1)
        {
            // Tạo Identity Users và Members
            var users = new List<(string email, string password, string fullName, double rank, string role)>
            {
                ("admin@pcm.vn", "Admin123", "Nguyễn Văn Admin", 5.5, "Admin"),
                ("ducminh@pcm.vn", "Member123", "Đức Minh", 3.2, "Member"),
                ("thuha@pcm.vn", "Member123", "Thu Hà", 2.8, "Member"),
                ("vanlong@pcm.vn", "Member123", "Văn Long", 4.1, "Member"),
                ("mylinh@pcm.vn", "Member123", "Mỹ Linh", 3.5, "Member"),
                ("hoangan@pcm.vn", "Member123", "Hoàng An", 2.3, "Member"),
                ("kimnga@pcm.vn", "Member123", "Kim Nga", 3.9, "Member"),
                ("quoctuan@pcm.vn", "Member123", "Quốc Tuấn", 4.5, "Member")
            };

            var membersList = new List<PCM_396.Models.Member>();

            foreach (var (email, password, fullName, rank, role) in users)
            {
                var identityUser = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(identityUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser, role);

                    var member = new PCM_396.Models.Member
                    {
                        IdentityUserId = identityUser.Id,
                        FullName = fullName,
                        Status = "Active",
                        RankLevel = rank
                    };
                    membersList.Add(member);
                }
            }

            context.Members.AddRange(membersList);
            await context.SaveChangesAsync();

            // Tạo Challenges mẫu
            var challenges = new List<PCM_396.Models.Challenge>
            {
                new PCM_396.Models.Challenge
                {
                    CreatorId = membersList[1].Id,
                    Title = "Thách đấu cuối tuần - Ai dám nhận?",
                    Description = "Tôi chấp nửa trái, đấu Singles. Thứ 7 này 9AM tại sân CLB.",
                    Status = 0, // Open
                    CreatedDate = DateTime.Now.AddDays(-2)
                },
                new PCM_396.Models.Challenge
                {
                    CreatorId = membersList[2].Id,
                    Title = "Doubles 2vs2 - Tìm đối thủ mạnh",
                    Description = "Team mình rank 3.0+ cần tìm đối thủ xứng tầm cho trận Doubles.",
                    Status = 1, // Accepted
                    CreatedDate = DateTime.Now.AddDays(-3)
                },
                new PCM_396.Models.Challenge
                {
                    CreatorId = membersList[3].Id,
                    Title = "Challenge cho người mới - Friendly match",
                    Description = "Trận giao hữu không tính điểm, ai mới chơi đều được nhận.",
                    Status = 0, // Open
                    CreatedDate = DateTime.Now.AddDays(-1)
                },
                new PCM_396.Models.Challenge
                {
                    CreatorId = membersList[4].Id,
                    Title = "Đấu tranh top 1 - Rank 4.0+",
                    Description = "Chỉ nhận thách đấu từ người có rank 4.0 trở lên. Có giải thưởng!",
                    Status = 2, // Completed
                    CreatedDate = DateTime.Now.AddDays(-5)
                }
            };

            context.Challenges.AddRange(challenges);
            await context.SaveChangesAsync();

            // Tạo Matches mẫu
            var matches = new List<PCM_396.Models.Match>
            {
                // Match Singles
                new PCM_396.Models.Match
                {
                    MatchDate = DateTime.Now.AddDays(-4),
                    Format = 0, // Singles
                    IsRanked = true,
                    ChallengeId = challenges[3].Id,
                    Winner1Id = membersList[4].Id,
                    Loser1Id = membersList[3].Id
                },
                // Match Doubles
                new PCM_396.Models.Match
                {
                    MatchDate = DateTime.Now.AddDays(-3),
                    Format = 1, // Doubles
                    IsRanked = true,
                    ChallengeId = challenges[1].Id,
                    Winner1Id = membersList[5].Id,
                    Winner2Id = membersList[6].Id,
                    Loser1Id = membersList[1].Id,
                    Loser2Id = membersList[2].Id
                },
                // Match Singles không tính điểm
                new PCM_396.Models.Match
                {
                    MatchDate = DateTime.Now.AddDays(-2),
                    Format = 0, // Singles
                    IsRanked = false,
                    Winner1Id = membersList[7].Id,
                    Loser1Id = membersList[5].Id
                }
            };

            context.Matches.AddRange(matches);
            await context.SaveChangesAsync();

            // Tạo Bookings mẫu
            var bookings = new List<PCM_396.Models.Booking>
            {
                new PCM_396.Models.Booking
                {
                    MemberId = membersList[1].Id,
                    StartTime = DateTime.Now.AddDays(1).Date.AddHours(9),
                    EndTime = DateTime.Now.AddDays(1).Date.AddHours(11),
                    CreatedDate = DateTime.Now.AddHours(-2)
                },
                new PCM_396.Models.Booking
                {
                    MemberId = membersList[2].Id,
                    StartTime = DateTime.Now.AddDays(1).Date.AddHours(14),
                    EndTime = DateTime.Now.AddDays(1).Date.AddHours(16),
                    CreatedDate = DateTime.Now.AddHours(-3)
                },
                new PCM_396.Models.Booking
                {
                    MemberId = membersList[3].Id,
                    StartTime = DateTime.Now.AddDays(2).Date.AddHours(8),
                    EndTime = DateTime.Now.AddDays(2).Date.AddHours(10),
                    CreatedDate = DateTime.Now.AddHours(-1)
                },
                new PCM_396.Models.Booking
                {
                    MemberId = membersList[4].Id,
                    StartTime = DateTime.Now.AddDays(2).Date.AddHours(16),
                    EndTime = DateTime.Now.AddDays(2).Date.AddHours(18),
                    CreatedDate = DateTime.Now.AddMinutes(-30)
                }
            };

            context.Bookings.AddRange(bookings);
            await context.SaveChangesAsync();

            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("✅ Seed data completed successfully!");
            logger.LogInformation("📊 Created: {MemberCount} members, {ChallengeCount} challenges, {MatchCount} matches, {BookingCount} bookings",
                membersList.Count, challenges.Count, matches.Count, bookings.Count);
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding data.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
