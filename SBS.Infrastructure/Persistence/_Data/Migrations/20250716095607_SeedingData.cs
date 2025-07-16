using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SBS.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_ResourceType_TypeId",
                table: "Resources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceType",
                table: "ResourceType");

            migrationBuilder.RenameTable(
                name: "ResourceType",
                newName: "ResourceTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceTypes",
                table: "ResourceTypes",
                column: "Id");

            migrationBuilder.InsertData(
                table: "BookingStatuses",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, false, "Upcoming" },
                    { 2, false, "Happening" },
                    { 3, false, "Finished" }
                });

            migrationBuilder.InsertData(
                table: "ResourceTypes",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, false, "MeetingRoom" },
                    { 2, false, "Desk" }
                });

            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "EndTime", "IsActive", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "StartTime" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6935), "Seeder", new TimeSpan(0, 10, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6936), "Seeder", new TimeSpan(0, 9, 0, 0, 0) },
                    { 2, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6941), "Seeder", new TimeSpan(0, 11, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6942), "Seeder", new TimeSpan(0, 10, 0, 0, 0) },
                    { 3, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6945), "Seeder", new TimeSpan(0, 12, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6946), "Seeder", new TimeSpan(0, 11, 0, 0, 0) },
                    { 4, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6949), "Seeder", new TimeSpan(0, 13, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6949), "Seeder", new TimeSpan(0, 12, 0, 0, 0) },
                    { 5, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6952), "Seeder", new TimeSpan(0, 14, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6952), "Seeder", new TimeSpan(0, 13, 0, 0, 0) },
                    { 6, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6955), "Seeder", new TimeSpan(0, 15, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6955), "Seeder", new TimeSpan(0, 14, 0, 0, 0) },
                    { 7, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6958), "Seeder", new TimeSpan(0, 16, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6958), "Seeder", new TimeSpan(0, 15, 0, 0, 0) },
                    { 8, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6961), "Seeder", new TimeSpan(0, 17, 0, 0, 0), true, false, new DateTime(2025, 7, 16, 9, 56, 6, 710, DateTimeKind.Utc).AddTicks(6962), "Seeder", new TimeSpan(0, 16, 0, 0, 0) }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_ResourceTypes_TypeId",
                table: "Resources",
                column: "TypeId",
                principalTable: "ResourceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_ResourceTypes_TypeId",
                table: "Resources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceTypes",
                table: "ResourceTypes");

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ResourceTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ResourceTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.RenameTable(
                name: "ResourceTypes",
                newName: "ResourceType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceType",
                table: "ResourceType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_ResourceType_TypeId",
                table: "Resources",
                column: "TypeId",
                principalTable: "ResourceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
