using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DaysStay = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tours_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tours_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToursLandmarks",
                columns: table => new
                {
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LandmarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToursLandmarks", x => new { x.TourId, x.LandmarkId });
                    table.ForeignKey(
                        name: "FK_ToursLandmarks_Landmarks_LandmarkId",
                        column: x => x.LandmarkId,
                        principalTable: "Landmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToursLandmarks_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersTours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersTours_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersTours_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "acabf74e-6649-47ee-8577-86bdd5c73849", "AQAAAAIAAYagAAAAEBbCHANX301jFfR4OW8cNYlh6S5YLsARMGbu2tdtTsY10d/DCYWof9SUir/dNL8pSw==", "e0af0ec8-e2ca-4687-8252-2f9759748fb8" });

            migrationBuilder.InsertData(
                table: "Tours",
                columns: new[] { "Id", "DaysStay", "Description", "DestinationId", "HotelId", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("0f6cd9ef-202c-4b9d-8990-845cd0e7a708"), 5, "Day one visit : Fushimi Inari Taisha. Day two and three free days for exploration. Day four visit : Island of Honshu. Day five leave.", new Guid("c304fab0-e8e7-4d06-9ffb-607c502be4e6"), new Guid("f8e2efd0-0e54-4c6d-8582-3651fa46f7b3"), "https://www.smartmeetings.com/wp-content/uploads/2018/03/japan-1.jpg", "Explore Japan", 1500m },
                    { new Guid("2e01bf71-70b4-45ff-95a6-e1270ed10ad9"), 7, "Day one and two explore : Piramids of Giza. Day three and four free time. Day five and six explore : Valley of the Kings. Day seven leave.", new Guid("f2af18bd-0c37-45dd-b545-3ff2beea2473"), new Guid("d857605f-1a16-4539-afaf-5e42f604413b"), "https://www.traveltalktours.com/wp-content/smush-webp/2023/02/Great-Sphinx-of-Giza.jpeg.webp", "Explore Egypt", 1700m },
                    { new Guid("bb776480-2b10-4eff-9995-ebfe574ec833"), 5, "Day one explore : Niagara Falls. Day two free day. Day three explore : Statue of Liberty. Day four free day. Day five leave.", new Guid("bba3d9da-4843-448e-b0f0-ba9c60e91b08"), new Guid("873c1c6c-c0aa-4a3c-b7fc-fc93cb31254b"), "https://miro.medium.com/v2/resize:fit:800/1*93v0YEC9A7Q8-8PxPwrstA.jpeg", "Explore United States of America", 2000m },
                    { new Guid("ec31fe73-7caf-43a0-a1a9-7d09d335ffa6"), 5, "Day one explore : Colloseum , Day two and three free days. Day four explore : Leaning Tower of Pisa. Day five leave.", new Guid("7060a6cc-6942-4346-b2c0-0011337cbaff"), new Guid("e27408c7-41dc-4504-b9c9-79f1cfe92f83"), "https://imgcld.yatra.com/ytimages/image/upload/t_holidays_srplist_tablet_hc/v5879035672/Holidays/Greenland/Venice_ycTyfo.jpg", "Explore Italy", 1400m }
                });

            migrationBuilder.InsertData(
                table: "ToursLandmarks",
                columns: new[] { "LandmarkId", "TourId" },
                values: new object[,]
                {
                    { new Guid("26d2d910-86f5-419a-a88a-a5b57206a72b"), new Guid("0f6cd9ef-202c-4b9d-8990-845cd0e7a708") },
                    { new Guid("5a883558-99eb-4c1a-8250-fcd2af1049bf"), new Guid("0f6cd9ef-202c-4b9d-8990-845cd0e7a708") },
                    { new Guid("e1264931-2355-4271-a612-398ca2f84b15"), new Guid("2e01bf71-70b4-45ff-95a6-e1270ed10ad9") },
                    { new Guid("e896a347-9756-43b4-9791-dca8c5aeb84b"), new Guid("2e01bf71-70b4-45ff-95a6-e1270ed10ad9") },
                    { new Guid("847d8970-36dd-40da-af65-6f0d19162eb7"), new Guid("bb776480-2b10-4eff-9995-ebfe574ec833") },
                    { new Guid("c61d4179-3893-43f4-bdbd-0df0d5d1a777"), new Guid("bb776480-2b10-4eff-9995-ebfe574ec833") },
                    { new Guid("0a5bd069-b91b-49f7-acfe-2f0df5985028"), new Guid("ec31fe73-7caf-43a0-a1a9-7d09d335ffa6") },
                    { new Guid("6824566a-9f3d-406e-b74a-f49f8aab1196"), new Guid("ec31fe73-7caf-43a0-a1a9-7d09d335ffa6") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_DestinationId",
                table: "Tours",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_HotelId",
                table: "Tours",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_ToursLandmarks_LandmarkId",
                table: "ToursLandmarks",
                column: "LandmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTours_TourId",
                table: "UsersTours",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTours_UserId",
                table: "UsersTours",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToursLandmarks");

            migrationBuilder.DropTable(
                name: "UsersTours");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "951f241a-7e8a-4098-bf4f-50bc94b0dbe8", "AQAAAAIAAYagAAAAEN6arHv3mLhPJ9DpZjGOOxeyTwCg8o93FJFxJN9IlziAYiVkLa6ld8+seWx8huqTeA==", "b399946a-c7bc-4cbf-af20-e4bbd8d6b7ad" });
        }
    }
}
