using Microsoft.EntityFrameworkCore.Migrations;

namespace Votacao.Repository.Migrations
{
    public partial class IdentityEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "SEC_Identity",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "SEC_Identity");
        }
    }
}
