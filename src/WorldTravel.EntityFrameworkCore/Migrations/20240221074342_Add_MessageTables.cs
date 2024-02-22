using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldTravel.Migrations
{
    public partial class Add_MessageTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderStatus = table.Column<int>(type: "int", nullable: false),
                    ReceiverStatus = table.Column<int>(type: "int", nullable: false),
                    SenderStatusDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiverStatusDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMessageContents_AppReceivedMessages",
                        column: x => x.ReceiverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppMessageContents_AppSendedMessages",
                        column: x => x.SenderId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppMessageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false),
                    IsDeletedForSender = table.Column<bool>(type: "bit", nullable: false),
                    IsDeletedForReceiver = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDateForSender = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDateForReceived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMessageContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMessageContents_AppMessages",
                        column: x => x.MessageId,
                        principalTable: "AppMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppMessageContent_MessageId",
                table: "AppMessageContents",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMessage_ReceiverId",
                table: "AppMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMessage_SenderId",
                table: "AppMessages",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppMessageContents");

            migrationBuilder.DropTable(
                name: "AppMessages");
        }
    }
}
