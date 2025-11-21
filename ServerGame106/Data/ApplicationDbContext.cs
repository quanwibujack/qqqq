using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Thêm cái này
using Microsoft.EntityFrameworkCore;
using ServerGame106.Models; // Thêm cái này để nhận diện ApplicationUser

namespace ServerGame106.Data
{
    // Sửa dòng này: kế thừa IdentityDbContext<ApplicationUser>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GameLevel> GameLevels { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<LevelResult> LevelResults { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Dòng này QUAN TRỌNG để Identity hoạt động đúng!

            // 1. Tạo dữ liệu mẫu cho GameLevel
            modelBuilder.Entity<GameLevel>().HasData(
                new GameLevel { LevelId = 1, Title = "Cấp độ 1", Description = "Dễ" },
                new GameLevel { LevelId = 2, Title = "Cấp độ 2", Description = "Trung bình" },
                new GameLevel { LevelId = 3, Title = "Cấp độ 3", Description = "Khó" }
            );

            // 2. Tạo dữ liệu mẫu cho Region
            modelBuilder.Entity<Region>().HasData(
                new Region { RegionId = 1, Name = "Đồng bằng sông Hồng" },
                new Region { RegionId = 2, Name = "Đồng bằng sông Cửu Long" }
            );

            // 3. Tạo dữ liệu mẫu cho Question
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    QuestionId = 1,
                    ContentQuestion = "Câu hỏi số 1",
                    Answer = "A",
                    Option1 = "A",
                    Option2 = "B",
                    Option3 = "C",
                    Option4 = "D",
                    LevelId = 1
                },
                new Question
                {
                    QuestionId = 2,
                    ContentQuestion = "Câu hỏi số 2",
                    Answer = "B",
                    Option1 = "A",
                    Option2 = "B",
                    Option3 = "C",
                    Option4 = "D",
                    LevelId = 2
                }
            );
        }

        // Thêm dòng này
        

    }
}