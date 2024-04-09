using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftTicketApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "85c00cf9-8b1d-4b6c-bd0b-f599f9fc5d1f", "06f888ba-a740-4e7f-95ce-c9cf9cf7b44c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85c00cf9-8b1d-4b6c-bd0b-f599f9fc5d1f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06f888ba-a740-4e7f-95ce-c9cf9cf7b44c");

            migrationBuilder.CreateTable(
                name: "TicketAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    TechnicianId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketAssignments_AspNetUsers_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketAssignments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2ac61b6d-f75a-486d-bcbe-021ae0fb9bed", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5a2db4ef-bd47-426c-aebb-863818d7fce7", 0, "e7241d76-b2b2-458c-83d5-5b32ef8eb0f7", "IdentityUser", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEGpMnIMRvDCr8FkuMN6XVlvcTLfG6fR2v6NeduA/Wgy8LPNKnkgQlgw7N8ORCazZAA==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2ac61b6d-f75a-486d-bcbe-021ae0fb9bed", "5a2db4ef-bd47-426c-aebb-863818d7fce7" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignments_TechnicianId",
                table: "TicketAssignments",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketAssignments");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2ac61b6d-f75a-486d-bcbe-021ae0fb9bed", "5a2db4ef-bd47-426c-aebb-863818d7fce7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ac61b6d-f75a-486d-bcbe-021ae0fb9bed");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a2db4ef-bd47-426c-aebb-863818d7fce7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "85c00cf9-8b1d-4b6c-bd0b-f599f9fc5d1f", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "06f888ba-a740-4e7f-95ce-c9cf9cf7b44c", 0, "64128735-1cb4-4eef-b60c-61ad353d5e0d", "IdentityUser", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEFS6Jw38aedqh47kZ43zK3OdKidvfjiiCjyWKXwU6EIWBs82m8/qKlGdBOeyDTnNnA==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "85c00cf9-8b1d-4b6c-bd0b-f599f9fc5d1f", "06f888ba-a740-4e7f-95ce-c9cf9cf7b44c" });
        }
    }
}
