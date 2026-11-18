using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalendarApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnumLookupTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvitationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurrenceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareAccessLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareAccessLevels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InvitationStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Pending" },
                    { 1, "Accepted" },
                    { 2, "Declined" },
                    { 3, "Tentative" }
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "EventReminder" },
                    { 1, "EventInvitation" },
                    { 2, "InvitationResponse" },
                    { 3, "CalendarShared" },
                    { 4, "System" }
                });

            migrationBuilder.InsertData(
                table: "RecurrenceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Daily" },
                    { 2, "Weekly" },
                    { 3, "Monthly" },
                    { 4, "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "ShareAccessLevels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ReadOnly" },
                    { 1, "ReadWrite" },
                    { 2, "Manage" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Type",
                table: "Notifications",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_Status",
                table: "Invitations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Recurrence",
                table: "Events",
                column: "Recurrence");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarShares_AccessLevel",
                table: "CalendarShares",
                column: "AccessLevel");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarShares_ShareAccessLevels_AccessLevel",
                table: "CalendarShares",
                column: "AccessLevel",
                principalTable: "ShareAccessLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_RecurrenceTypes_Recurrence",
                table: "Events",
                column: "Recurrence",
                principalTable: "RecurrenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_InvitationStatuses_Status",
                table: "Invitations",
                column: "Status",
                principalTable: "InvitationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_Type",
                table: "Notifications",
                column: "Type",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarShares_ShareAccessLevels_AccessLevel",
                table: "CalendarShares");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_RecurrenceTypes_Recurrence",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_InvitationStatuses_Status",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_Type",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "InvitationStatuses");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "RecurrenceTypes");

            migrationBuilder.DropTable(
                name: "ShareAccessLevels");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Type",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_Status",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Events_Recurrence",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_CalendarShares_AccessLevel",
                table: "CalendarShares");
        }
    }
}
