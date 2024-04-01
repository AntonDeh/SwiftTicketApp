using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftTicketApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "59b1c2ad-51e6-4cba-ac55-b37f30c4d0c2", "0110e62d-e315-457e-8648-49dcad6df1c4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59b1c2ad-51e6-4cba-ac55-b37f30c4d0c2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0110e62d-e315-457e-8648-49dcad6df1c4");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentSite",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LabLocation",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Urgency",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "745a1cbe-6fef-4a04-9ecd-c3b72f642132", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2b436256-a368-4ee4-9122-8cdf0224d3e9", 0, "40a2e83e-cbe7-419f-ba7c-b42b1c06f2ea", "IdentityUser", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAELxqObC4D/vrOfosF56r3nRQCYC9PeteacEUT8CFzReU6ko7C9YpeS94UeUi3GvwUA==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "745a1cbe-6fef-4a04-9ecd-c3b72f642132", "2b436256-a368-4ee4-9122-8cdf0224d3e9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "745a1cbe-6fef-4a04-9ecd-c3b72f642132", "2b436256-a368-4ee4-9122-8cdf0224d3e9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "745a1cbe-6fef-4a04-9ecd-c3b72f642132");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2b436256-a368-4ee4-9122-8cdf0224d3e9");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CurrentSite",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LabLocation",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Urgency",
                table: "Tickets");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "59b1c2ad-51e6-4cba-ac55-b37f30c4d0c2", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0110e62d-e315-457e-8648-49dcad6df1c4", 0, "266c892f-75d8-4470-ae32-f45ec078da68", "IdentityUser", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEFGSgMBJNfeAb1TtL/zKNEbiqZIKhYF6yNCOETLaV25kLFLXCHanijD58x4kLRvV+w==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "59b1c2ad-51e6-4cba-ac55-b37f30c4d0c2", "0110e62d-e315-457e-8648-49dcad6df1c4" });
        }
    }
}
