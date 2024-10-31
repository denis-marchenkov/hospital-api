using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Hospital");

            migrationBuilder.CreateTable(
                name: "Patients",
                schema: "Hospital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Names",
                schema: "Hospital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Use = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Names", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Names_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "Hospital",
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GivenNames",
                schema: "Hospital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GivenNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GivenNames_Names_NameId",
                        column: x => x.NameId,
                        principalSchema: "Hospital",
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Hospital",
                table: "Patients",
                columns: new[] { "Id", "Active", "BirthDate", "Gender" },
                values: new object[] { new Guid("b0710e24-d505-401c-b8c3-6f161e13b6a7"), true, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male" });

            migrationBuilder.InsertData(
                schema: "Hospital",
                table: "Names",
                columns: new[] { "Id", "Family", "PatientId", "Use" },
                values: new object[] { new Guid("b83dc8ef-c2ed-43bd-81d5-3bc6d0049a26"), "family", new Guid("b0710e24-d505-401c-b8c3-6f161e13b6a7"), "use" });

            migrationBuilder.InsertData(
                schema: "Hospital",
                table: "GivenNames",
                columns: new[] { "Id", "NameId", "Value" },
                values: new object[] { new Guid("42169402-5157-4947-81e5-8bcdb8510a46"), new Guid("b83dc8ef-c2ed-43bd-81d5-3bc6d0049a26"), "given1" });

            migrationBuilder.InsertData(
                schema: "Hospital",
                table: "GivenNames",
                columns: new[] { "Id", "NameId", "Value" },
                values: new object[] { new Guid("c9f4bcfb-b1e5-49d4-8fbe-19b691935d40"), new Guid("b83dc8ef-c2ed-43bd-81d5-3bc6d0049a26"), "given2" });

            migrationBuilder.CreateIndex(
                name: "IX_GivenNames_NameId",
                schema: "Hospital",
                table: "GivenNames",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Names_PatientId",
                schema: "Hospital",
                table: "Names",
                column: "PatientId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GivenNames",
                schema: "Hospital");

            migrationBuilder.DropTable(
                name: "Names",
                schema: "Hospital");

            migrationBuilder.DropTable(
                name: "Patients",
                schema: "Hospital");
        }
    }
}
