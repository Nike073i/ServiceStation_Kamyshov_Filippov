using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceStationDatabaseImplement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIO = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarName = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SparePartName = table.Column<string>(nullable: false),
                    SparePartPrice = table.Column<decimal>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpareParts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalMaintenances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicalMaintenanceName = table.Column<string>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalMaintenances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkName = table.Column<string>(nullable: false),
                    WorkPrice = table.Column<decimal>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarSpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SparePartId = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarSpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarSpareParts_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarSpareParts_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRecordings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatePassed = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    TechnicalMaintenanceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRecordings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRecordings_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRecordings_TechnicalMaintenances_TechnicalMaintenanceId",
                        column: x => x.TechnicalMaintenanceId,
                        principalTable: "TechnicalMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRecordings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalMaintenanceCars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicalMaintenanceId = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalMaintenanceCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalMaintenanceCars_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicalMaintenanceCars_TechnicalMaintenances_TechnicalMaintenanceId",
                        column: x => x.TechnicalMaintenanceId,
                        principalTable: "TechnicalMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalMaintenanceWorks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(nullable: false),
                    TechnicalMaintenanceId = table.Column<int>(nullable: false),
                    WorkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalMaintenanceWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalMaintenanceWorks_TechnicalMaintenances_TechnicalMaintenanceId",
                        column: x => x.TechnicalMaintenanceId,
                        principalTable: "TechnicalMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicalMaintenanceWorks_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkDurations",
                columns: table => new
                {
                    WorkId = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDurations", x => x.WorkId);
                    table.ForeignKey(
                        name: "FK_WorkDurations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkDurations_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkSpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(nullable: false),
                    SparePartId = table.Column<int>(nullable: false),
                    WorkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSpareParts_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkSpareParts_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UserId",
                table: "Cars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSpareParts_CarId",
                table: "CarSpareParts",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSpareParts_SparePartId",
                table: "CarSpareParts",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRecordings_CarId",
                table: "ServiceRecordings",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRecordings_TechnicalMaintenanceId",
                table: "ServiceRecordings",
                column: "TechnicalMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRecordings_UserId",
                table: "ServiceRecordings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_UserId",
                table: "SpareParts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalMaintenanceCars_CarId",
                table: "TechnicalMaintenanceCars",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalMaintenanceCars_TechnicalMaintenanceId",
                table: "TechnicalMaintenanceCars",
                column: "TechnicalMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalMaintenances_UserId",
                table: "TechnicalMaintenances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalMaintenanceWorks_TechnicalMaintenanceId",
                table: "TechnicalMaintenanceWorks",
                column: "TechnicalMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalMaintenanceWorks_WorkId",
                table: "TechnicalMaintenanceWorks",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDurations_UserId",
                table: "WorkDurations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_UserId",
                table: "Works",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpareParts_SparePartId",
                table: "WorkSpareParts",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpareParts_WorkId",
                table: "WorkSpareParts",
                column: "WorkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarSpareParts");

            migrationBuilder.DropTable(
                name: "ServiceRecordings");

            migrationBuilder.DropTable(
                name: "TechnicalMaintenanceCars");

            migrationBuilder.DropTable(
                name: "TechnicalMaintenanceWorks");

            migrationBuilder.DropTable(
                name: "WorkDurations");

            migrationBuilder.DropTable(
                name: "WorkSpareParts");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "TechnicalMaintenances");

            migrationBuilder.DropTable(
                name: "SpareParts");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
