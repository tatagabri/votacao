using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Votacao.Repository.Migrations
{
    public partial class SystemEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_Candidate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Candidate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Election",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Election", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ElectionCandidate",
                columns: table => new
                {
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ElectionCandidate", x => new { x.CandidateId, x.ElectionId });
                    table.ForeignKey(
                        name: "FK_SYS_ElectionCandidate_SYS_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "SYS_Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_ElectionCandidate_SYS_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "SYS_Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Vote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    VoterId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: true),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SYS_Vote_SEC_Identity_VoterId",
                        column: x => x.VoterId,
                        principalTable: "SEC_Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_Vote_SYS_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "SYS_Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_Vote_SYS_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "SYS_Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ElectionCandidate_ElectionId",
                table: "SYS_ElectionCandidate",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Vote_CandidateId",
                table: "SYS_Vote",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Vote_ElectionId",
                table: "SYS_Vote",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Vote_VoterId",
                table: "SYS_Vote",
                column: "VoterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_ElectionCandidate");

            migrationBuilder.DropTable(
                name: "SYS_Vote");

            migrationBuilder.DropTable(
                name: "SYS_Candidate");

            migrationBuilder.DropTable(
                name: "SYS_Election");
        }
    }
}
