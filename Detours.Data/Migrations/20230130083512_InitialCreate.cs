using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detours.Data.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "TourLocations",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					X = table.Column<float>(type: "real", nullable: false),
					Y = table.Column<float>(type: "real", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TourLocations", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Tours",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
					Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					Duration = table.Column<int>(type: "int", nullable: false),
					MaxGroupSize = table.Column<int>(type: "int", nullable: false),
					Difficulty = table.Column<int>(type: "int", nullable: false),
					Price = table.Column<decimal>(type: "money", nullable: false),
					Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Tours", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Role = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "StartTourLocations",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_StartTourLocations", x => x.Id);
					table.ForeignKey(
						name: "FK_StartTourLocations_TourLocations_Id",
						column: x => x.Id,
						principalTable: "TourLocations",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_StartTourLocations_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "TourImageCovers",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TourImageCovers", x => x.Id);
					table.ForeignKey(
						name: "FK_TourImageCovers_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "TourImages",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TourImages", x => x.Id);
					table.ForeignKey(
						name: "FK_TourImages_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "TourLocationsByDay",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Day = table.Column<byte>(type: "tinyint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TourLocationsByDay", x => x.Id);
					table.ForeignKey(
						name: "FK_TourLocationsByDay_TourLocations_Id",
						column: x => x.Id,
						principalTable: "TourLocations",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_TourLocationsByDay_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "TourStartDates",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TourStartDates", x => x.Id);
					table.ForeignKey(
						name: "FK_TourStartDates_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Bookings",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Price = table.Column<decimal>(type: "money", nullable: false),
					CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Bookings", x => x.Id);
					table.ForeignKey(
						name: "FK_Bookings_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Bookings_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "RefreshTokens",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					JwtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
					ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
					Used = table.Column<bool>(type: "bit", nullable: false),
					Invalidated = table.Column<bool>(type: "bit", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RefreshTokens", x => x.Id);
					table.ForeignKey(
						name: "FK_RefreshTokens_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Reviews",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Rating = table.Column<byte>(type: "tinyint", nullable: false),
					Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Reviews", x => x.Id);
					table.ForeignKey(
						name: "FK_Reviews_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Reviews_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "TourGuides",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					GuideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TourGuides", x => x.Id);
					table.ForeignKey(
						name: "FK_TourGuides_Tours_TourId",
						column: x => x.TourId,
						principalTable: "Tours",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_TourGuides_Users_GuideId",
						column: x => x.GuideId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Bookings_TourId",
				table: "Bookings",
				column: "TourId");

			migrationBuilder.CreateIndex(
				name: "IX_Bookings_UserId",
				table: "Bookings",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_RefreshTokens_UserId",
				table: "RefreshTokens",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Reviews_TourId",
				table: "Reviews",
				column: "TourId");

			migrationBuilder.CreateIndex(
				name: "IX_Reviews_UserId",
				table: "Reviews",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_StartTourLocations_TourId",
				table: "StartTourLocations",
				column: "TourId",
				unique: true,
				filter: "[TourId] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_TourGuides_GuideId",
				table: "TourGuides",
				column: "GuideId");

			migrationBuilder.CreateIndex(
				name: "IX_TourGuides_TourId_GuideId",
				table: "TourGuides",
				columns: new[] { "TourId", "GuideId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_TourImageCovers_TourId",
				table: "TourImageCovers",
				column: "TourId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_TourImages_TourId",
				table: "TourImages",
				column: "TourId");

			migrationBuilder.CreateIndex(
				name: "IX_TourLocationsByDay_TourId",
				table: "TourLocationsByDay",
				column: "TourId");

			migrationBuilder.CreateIndex(
				name: "IX_Tours_Name",
				table: "Tours",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Tours_Slug",
				table: "Tours",
				column: "Slug",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_TourStartDates_TourId_Date",
				table: "TourStartDates",
				columns: new[] { "TourId", "Date" },
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Bookings");

			migrationBuilder.DropTable(
				name: "RefreshTokens");

			migrationBuilder.DropTable(
				name: "Reviews");

			migrationBuilder.DropTable(
				name: "StartTourLocations");

			migrationBuilder.DropTable(
				name: "TourGuides");

			migrationBuilder.DropTable(
				name: "TourImageCovers");

			migrationBuilder.DropTable(
				name: "TourImages");

			migrationBuilder.DropTable(
				name: "TourLocationsByDay");

			migrationBuilder.DropTable(
				name: "TourStartDates");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.DropTable(
				name: "TourLocations");

			migrationBuilder.DropTable(
				name: "Tours");
		}
	}
}
