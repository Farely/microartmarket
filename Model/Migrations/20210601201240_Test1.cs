using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class Test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtTags",
                columns: table => new
                {
                    ArtTagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtTags", x => x.ArtTagId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    DescriptionText = table.Column<string>(type: "text", nullable: true),
                    ShortStatus = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserArtTag",
                columns: table => new
                {
                    UserArtTagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true),
                    ArtTagId = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserArtTag", x => x.UserArtTagId);
                    table.ForeignKey(
                        name: "FK_UserArtTag_ArtTags_ArtTagId",
                        column: x => x.ArtTagId,
                        principalTable: "ArtTags",
                        principalColumn: "ArtTagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserArtTag_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: true),
                    FileId = table.Column<string>(type: "text", nullable: true),
                    OriginalFileName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    ArtState = table.Column<int>(type: "integer", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResult",
                columns: table => new
                {
                    TestResultId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestId = table.Column<int>(type: "integer", nullable: true),
                    IsEnded = table.Column<bool>(type: "boolean", nullable: false),
                    TestedUserId = table.Column<string>(type: "text", nullable: true),
                    SuggestedAuthorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResult", x => x.TestResultId);
                    table.ForeignKey(
                        name: "FK_TestResult_AspNetUsers_SuggestedAuthorId",
                        column: x => x.SuggestedAuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestResult_AspNetUsers_TestedUserId",
                        column: x => x.TestedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestResult_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtTagArtWork",
                columns: table => new
                {
                    ArtWorksId = table.Column<int>(type: "integer", nullable: false),
                    TagsArtTagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtTagArtWork", x => new { x.ArtWorksId, x.TagsArtTagId });
                    table.ForeignKey(
                        name: "FK_ArtTagArtWork_ArtTags_TagsArtTagId",
                        column: x => x.TagsArtTagId,
                        principalTable: "ArtTags",
                        principalColumn: "ArtTagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtTagArtWork_Works_ArtWorksId",
                        column: x => x.ArtWorksId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsOver = table.Column<bool>(type: "boolean", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: true),
                    ReviewFromCustomer = table.Column<string>(type: "text", nullable: true),
                    DescriptionFromCustomer = table.Column<string>(type: "text", nullable: true),
                    ArtId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId1 = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    AuthorId1 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_AuthorId1",
                        column: x => x.AuthorId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Works_ArtId",
                        column: x => x.ArtId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtTagId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answers_ArtTags_ArtTagId",
                        column: x => x.ArtTagId,
                        principalTable: "ArtTags",
                        principalColumn: "ArtTagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerId = table.Column<int>(type: "integer", nullable: true),
                    TestResultId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerResult_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerResult_TestResult_TestResultId",
                        column: x => x.TestResultId,
                        principalTable: "TestResult",
                        principalColumn: "TestResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ArtTags",
                columns: new[] { "ArtTagId", "Description", "Label" },
                values: new object[,]
                {
                    { 1, "Арт-реализм", "Арт-реализм" },
                    { 2, "Арт-фэнтези", "Арт-фэнтези" },
                    { 3, "Арт-киберпанк", "Арт-киберпанк" },
                    { 4, "Арт-другое", "Арт-другое" },
                    { 5, "Оформление-ярко", "Оформление-ярко" },
                    { 6, "Оформление-минимализм", "Оформление-минимализм" },
                    { 7, "Оформление-YT/Twitch", "Оформление-YT/Twitch" },
                    { 8, "Оформление-Inst/Twitter", "Оформление-Inst/Twitter" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bcd8095a-f92f-4666-97d6-91a032ee27b5", "5519d282-94b3-44b6-8704-afeccec7d1e9", "Admin", "ADMIN" },
                    { "726ce49e-a47c-4715-98c7-e3c943db1c3e", "53e88b1b-c44d-4bb7-abe7-91b8148e18fb", "Customer", "CUSTOMER" },
                    { "8805cf28-dd66-46eb-967d-085e00e4a6b1", "06f88c2a-54d0-495a-881e-4aeb78327763", "Artist", "ARTIST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DescriptionText", "DisplayName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShortStatus", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "63e79bc9-8b92-4d5b-84ff-f543b8a0f620", 0, "e4d01129-824f-4db0-aa17-5b5da1fae9ea", null, "User1", null, false, false, null, null, "USER1", "AQAAAAEAACcQAAAAEMScFuvvP2KzYT5N0BS2DiNx+rNxWwieWNUt/0kH1oFXiTXBEqeCLEJdGIAJsiyMaQ==", null, false, "", null, false, "User1" },
                    { "2f2ae47c-998e-4f21-b906-ca01ee340908", 0, "93d3c99c-52a5-4c02-a6eb-c95a3bff949a", null, "User2", null, false, false, null, null, "USER2", "AQAAAAEAACcQAAAAEKhQ2c4ewRLwgNr/ZbJf+h4OvqOjBpmHBXREbJGNz4yXLzGOFuN6JKtih75oOFcsKg==", null, false, "", null, false, "User2" },
                    { "4f12a2cd-bf66-495b-ac86-dd52a27ed35d", 0, "4d0b211d-b6ed-4d5a-9072-2743e2711235", null, "User3", null, false, false, null, null, "USER3", "AQAAAAEAACcQAAAAEFHGb4HN4PgMzGvUMws1Gu8h1MpXN/9AYN8/RBvLnwxKB3jiHyMUQk+aLP2MX5UDMQ==", null, false, "", null, false, "User3" },
                    { "6c147f60-d2aa-4781-a562-fc663a438b8d", 0, "d851efb6-d6e5-4eef-9577-6a07e685cc6c", null, "User4", null, false, false, null, null, "USER4", "AQAAAAEAACcQAAAAEPT/txEf9r0RvMGEIHt3XScstoAuX7IetkNCnkkgaRFiojenxtnQZrfMlgFW1gwtEw==", null, false, "", null, false, "User4" }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "Description", "Label" },
                values: new object[] { 1, "Тестовый тест", "Теста намба ван" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "bcd8095a-f92f-4666-97d6-91a032ee27b5", "63e79bc9-8b92-4d5b-84ff-f543b8a0f620" },
                    { "8805cf28-dd66-46eb-967d-085e00e4a6b1", "2f2ae47c-998e-4f21-b906-ca01ee340908" },
                    { "8805cf28-dd66-46eb-967d-085e00e4a6b1", "4f12a2cd-bf66-495b-ac86-dd52a27ed35d" },
                    { "726ce49e-a47c-4715-98c7-e3c943db1c3e", "6c147f60-d2aa-4781-a562-fc663a438b8d" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionId", "TestId", "Text" },
                values: new object[] { 1, 1, "Первый вопрос" });

            migrationBuilder.InsertData(
                table: "UserArtTag",
                columns: new[] { "UserArtTagId", "ApplicationUserId", "ArtTagId", "Rate" },
                values: new object[,]
                {
                    { 1, "63e79bc9-8b92-4d5b-84ff-f543b8a0f620", 1, 4 },
                    { 2, "2f2ae47c-998e-4f21-b906-ca01ee340908", 2, 4 },
                    { 3, "2f2ae47c-998e-4f21-b906-ca01ee340908", 3, 4 },
                    { 4, "4f12a2cd-bf66-495b-ac86-dd52a27ed35d", 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "AnswerId", "ArtTagId", "QuestionId", "Rate", "Text" },
                values: new object[,]
                {
                    { 1, 1, 1, 3, "Ответ в пользу первого тега" },
                    { 2, 2, 1, 4, "Ответ в пользу второго тега" },
                    { 3, 3, 1, 2, "Ответ в пользу третьего тега" },
                    { 4, 4, 1, 4, "Ответ в пользу четвертого тега" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerResult_AnswerId",
                table: "AnswerResult",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerResult_TestResultId",
                table: "AnswerResult",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ArtTagId",
                table: "Answers",
                column: "ArtTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtTagArtWork_TagsArtTagId",
                table: "ArtTagArtWork",
                column: "TagsArtTagId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ArtId",
                table: "Orders",
                column: "ArtId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AuthorId1",
                table: "Orders",
                column: "AuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId1",
                table: "Orders",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_SuggestedAuthorId",
                table: "TestResult",
                column: "SuggestedAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_TestedUserId",
                table: "TestResult",
                column: "TestedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_TestId",
                table: "TestResult",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserArtTag_ApplicationUserId",
                table: "UserArtTag",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserArtTag_ArtTagId",
                table: "UserArtTag",
                column: "ArtTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_AuthorId",
                table: "Works",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerResult");

            migrationBuilder.DropTable(
                name: "ArtTagArtWork");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "UserArtTag");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "TestResult");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "ArtTags");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tests");
        }
    }
}
