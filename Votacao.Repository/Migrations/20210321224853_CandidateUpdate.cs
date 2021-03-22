using Microsoft.EntityFrameworkCore.Migrations;

namespace Votacao.Repository.Migrations
{
    public partial class CandidateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "SYS_Candidate");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "SYS_Candidate",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "SYS_Candidate",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "SYS_Candidate",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Party",
                table: "SYS_Candidate",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Candidate_Code",
                table: "SYS_Candidate",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_Identity_CPF",
                table: "SEC_Identity",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_Identity_Email",
                table: "SEC_Identity",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SYS_Candidate_Code",
                table: "SYS_Candidate");

            migrationBuilder.DropIndex(
                name: "IX_SEC_Identity_CPF",
                table: "SEC_Identity");

            migrationBuilder.DropIndex(
                name: "IX_SEC_Identity_Email",
                table: "SEC_Identity");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "SYS_Candidate");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "SYS_Candidate");

            migrationBuilder.DropColumn(
                name: "Party",
                table: "SYS_Candidate");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "SYS_Candidate",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "SYS_Candidate",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
