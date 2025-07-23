using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBS.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class JWTTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6832), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6833) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6839), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6839) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6841), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6842) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6843), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6844) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6845), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6846) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6847), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6848) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6850), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6850) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6852), new DateTime(2025, 7, 17, 15, 39, 11, 929, DateTimeKind.Utc).AddTicks(6852) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6935), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6936) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6941), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6942) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6945), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6946) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6949), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6949) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6952), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6952) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6955), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6955) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6958), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6958) });

            migrationBuilder.UpdateData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6961), new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6962) });
        }
    }
}
