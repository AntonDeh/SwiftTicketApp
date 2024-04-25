using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftTicketApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "854d57a1-5914-4212-951f-21d6b968db2e", "9d5888e2-0594-4fa5-8587-d5fb4d18aba7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "854d57a1-5914-4212-951f-21d6b968db2e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9d5888e2-0594-4fa5-8587-d5fb4d18aba7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "603e5792-a9c8-43ac-9971-2404dc2955cc", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "06e31909-838a-405d-90ff-3281390e3420", 0, "79b23ec2-1758-49e9-a569-e2b5707a830b", "User", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEECf9uqWDkQ22ZxuHdYJy2BDBIAWKv9ZCjFeiREKsRHDbMhw8VhWJkTVEMXqPDj7ag==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "603e5792-a9c8-43ac-9971-2404dc2955cc", "06e31909-838a-405d-90ff-3281390e3420" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "603e5792-a9c8-43ac-9971-2404dc2955cc", "06e31909-838a-405d-90ff-3281390e3420" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "603e5792-a9c8-43ac-9971-2404dc2955cc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e31909-838a-405d-90ff-3281390e3420");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "854d57a1-5914-4212-951f-21d6b968db2e", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9d5888e2-0594-4fa5-8587-d5fb4d18aba7", 0, "e7cd6800-b970-4df0-a5eb-ade0cf1674ac", "User", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEDb2dxYi/D6JUaBJx2dXOPX7c+PVmOuzqU/UqoM+am6jjYpZOIC439UNSDF9Pm8rkQ==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "854d57a1-5914-4212-951f-21d6b968db2e", "9d5888e2-0594-4fa5-8587-d5fb4d18aba7" });
        }
    }
}
