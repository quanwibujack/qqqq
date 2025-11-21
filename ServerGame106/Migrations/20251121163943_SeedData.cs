using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServerGame106.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameLevels",
                columns: new[] { "LevelId", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Dễ", "Cấp độ 1" },
                    { 2, "Trung bình", "Cấp độ 2" },
                    { 3, "Khó", "Cấp độ 3" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionId", "Answer", "ContentQuestion", "LevelId", "Option1", "Option2", "Option3", "Option4" },
                values: new object[,]
                {
                    { 1, "A", "Câu hỏi số 1", 1, "A", "B", "C", "D" },
                    { 2, "B", "Câu hỏi số 2", 2, "A", "B", "C", "D" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "RegionId", "Name" },
                values: new object[,]
                {
                    { 1, "Đồng bằng sông Hồng" },
                    { 2, "Đồng bằng sông Cửu Long" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "RegionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "RegionId",
                keyValue: 2);
        }
    }
}
