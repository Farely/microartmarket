using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedData;
using SharedData.TestModel;

namespace Model
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ArtWork> Works { get; set; }
        public DbSet<ArtTag> ArtTags { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestResult> TestResult { get; set; }
        public DbSet<AnswerResult> AnswerResult { get; set; }


        public DbSet<Order> Orders { get; set; }
        public override DbSet<ApplicationUser> Users { get; set; }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=artMarket;User ID=marketBack;Password=11111;Pooling=true;commandtimeout=0");
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var ArtTags = new[]
            {
                new ArtTag {ArtTagId = 1, Description = "Арт-реализм", Label = "Арт-реализм"},
                new ArtTag {ArtTagId = 2, Description = "Арт-фэнтези", Label = "Арт-фэнтези"},
                new ArtTag {ArtTagId = 3, Description = "Арт-киберпанк", Label = "Арт-киберпанк"},
                new ArtTag {ArtTagId = 4, Description = "Арт-другое", Label = "Арт-другое"},
                new ArtTag {ArtTagId = 5, Description = "Оформление-ярко", Label = "Оформление-ярко"},
                new ArtTag {ArtTagId = 6, Description = "Оформление-минимализм", Label = "Оформление-минимализм"},
                new ArtTag {ArtTagId = 7, Description = "Оформление-YT/Twitch", Label = "Оформление-YT/Twitch"},
                new ArtTag {ArtTagId = 8, Description = "Оформление-Inst/Twitter", Label = "Оформление-Inst/Twitter"}
            };

            var ADMIN_ID = Guid.NewGuid().ToString();
            var CUSTOMER_ID = Guid.NewGuid().ToString();
            var ARTIST_ID = Guid.NewGuid().ToString();

            var Roles = new[]
            {
                new IdentityRole
                {
                    Id = ADMIN_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = CUSTOMER_ID,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                },
                new IdentityRole
                {
                    Id = ARTIST_ID,
                    Name = "Artist",
                    NormalizedName = "ARTIST"
                }
            };

            var hasher = new PasswordHasher<ApplicationUser>();
            var USER1_ID = Guid.NewGuid().ToString();
            var USER2_ID = Guid.NewGuid().ToString();
            var USER3_ID = Guid.NewGuid().ToString();
            var USER4_ID = Guid.NewGuid().ToString();

            var Users = new[]
            {
                new ApplicationUser
                {
                    Id = USER1_ID,
                    DisplayName = "User1",
                    UserName = "User1",
                    NormalizedUserName = "USER1",
                    PasswordHash = hasher.HashPassword(null, USER1_ID),
                    SecurityStamp = string.Empty
                },
                new ApplicationUser
                {
                    Id = USER2_ID,
                    DisplayName = "User2",
                    UserName = "User2",
                    NormalizedUserName = "USER2",
                    PasswordHash = hasher.HashPassword(null, USER2_ID),
                    SecurityStamp = string.Empty
                },
                new ApplicationUser
                {
                    Id = USER3_ID,
                    DisplayName = "User3",
                    UserName = "User3",
                    NormalizedUserName = "USER3",
                    PasswordHash = hasher.HashPassword(null, USER3_ID),
                    SecurityStamp = string.Empty
                },
                new ApplicationUser
                {
                    Id = USER4_ID,
                    DisplayName = "User4",
                    NormalizedUserName = "USER4",
                    UserName = "User4",
                    PasswordHash = hasher.HashPassword(null, USER4_ID),
                    SecurityStamp = string.Empty
                }
            };

            var UsersRoles = new[]
            {
                new IdentityUserRole<string>
                {
                    RoleId = ADMIN_ID,
                    UserId = USER1_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = ARTIST_ID,
                    UserId = USER2_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = ARTIST_ID,
                    UserId = USER3_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = CUSTOMER_ID,
                    UserId = USER4_ID
                }
            };


            var SeedTest = new Test[]
            {
                new Test(){Description = "Тестовый тест",
                Id = 1,
                Label = "Теста намба ван"
                }
            };
            var Questions = new[]
            {
                new Question
                {
                    QuestionId = 1,
                    Text = "Первый вопрос",
                    TestId = SeedTest[0].Id
                }
            };
            var Answers = new[]
            {
                new Answer
                {
                    AnswerId = 1, QuestionId = Questions[0].QuestionId, Rate = 3, Text = "Ответ в пользу первого тега",
                    ArtTagId = ArtTags[0].ArtTagId
                },
                new Answer
                {
                    AnswerId = 2, QuestionId = Questions[0].QuestionId, Rate = 4, Text = "Ответ в пользу второго тега",
                    ArtTagId = ArtTags[1].ArtTagId
                },
                new Answer
                {
                    AnswerId = 3,QuestionId = Questions[0].QuestionId, Rate = 2, Text = "Ответ в пользу третьего тега",
                    ArtTagId = ArtTags[2].ArtTagId
                },
                new Answer
                {
                    AnswerId = 4, QuestionId = Questions[0].QuestionId, Rate = 4, Text = "Ответ в пользу четвертого тега",
                    ArtTagId = ArtTags[3].ArtTagId
                }
            };

          


            var UsersArtTags = new[]
            {
                new UserArtTag {UserArtTagId = 1, ApplicationUserId = USER1_ID, Rate = 4, ArtTagId = 1},
                new UserArtTag {UserArtTagId = 2, ApplicationUserId = USER2_ID, Rate = 4, ArtTagId = 2},
                new UserArtTag {UserArtTagId = 3, ApplicationUserId = USER2_ID, Rate = 4, ArtTagId = 3},
                new UserArtTag {UserArtTagId = 4, ApplicationUserId = USER3_ID, Rate = 4, ArtTagId = 4}
            };

         
           

            builder.Entity<ApplicationUser>().HasMany(u => u.Tags);
            builder.Entity<Test>().HasMany<Question>(t => t.Questions).WithOne();
        
            builder.Entity<Order>().HasOne<ApplicationUser>(o => o.Customer).WithMany();



            builder.Entity<Answer>().HasOne(a => a.Question).WithMany(q => q.Answers).HasForeignKey(a => a.QuestionId);
            builder.Entity<Answer>().HasOne(a => a.ArtTag).WithMany().HasForeignKey(a => a.ArtTagId);
            builder.Entity<Order>().HasOne(o => o.Art).WithOne(a => a.Order).HasForeignKey<Order>(o => o.ArtId);
            builder.Entity<Question>().HasOne(q => q.Test).WithMany(t => t.Questions).HasForeignKey(q => q.TestId);
         
            
            builder.Entity<IdentityRole>().HasData(Roles);
            builder.Entity<ApplicationUser>().HasData(Users);
            builder.Entity<Test>().HasData(SeedTest);
            builder.Entity<IdentityUserRole<string>>().HasData(UsersRoles);
            builder.Entity<Question>().HasData(Questions);
            builder.Entity<Answer>().HasData(Answers);
            builder.Entity<ArtTag>().HasData(ArtTags);
            builder.Entity<UserArtTag>().HasData(UsersArtTags);
           
            
            base.OnModelCreating(builder);
        }
    }
}