using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Votacao.Repository.Migrations
{
    public partial class Security : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SEC_Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_Identity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEC_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEC_IdentityClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_IdentityClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SEC_IdentityClaim_SEC_Identity_UserId",
                        column: x => x.UserId,
                        principalTable: "SEC_Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SEC_IdentityLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_IdentityLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_SEC_IdentityLogin_SEC_Identity_UserId",
                        column: x => x.UserId,
                        principalTable: "SEC_Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SEC_IdentityToken",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_IdentityToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_SEC_IdentityToken_SEC_Identity_UserId",
                        column: x => x.UserId,
                        principalTable: "SEC_Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SEC_IdentityRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_IdentityRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_SEC_IdentityRole_SEC_Identity_UserId",
                        column: x => x.UserId,
                        principalTable: "SEC_Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SEC_IdentityRole_SEC_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SEC_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SEC_RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EditionUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EditionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SEC_RoleClaim_SEC_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SEC_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "SEC_Identity",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "SEC_Identity",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_IdentityClaim_UserId",
                table: "SEC_IdentityClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_IdentityLogin_UserId",
                table: "SEC_IdentityLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_IdentityRole_RoleId",
                table: "SEC_IdentityRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "SEC_Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_RoleClaim_RoleId",
                table: "SEC_RoleClaim",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SEC_IdentityClaim");

            migrationBuilder.DropTable(
                name: "SEC_IdentityLogin");

            migrationBuilder.DropTable(
                name: "SEC_IdentityRole");

            migrationBuilder.DropTable(
                name: "SEC_IdentityToken");

            migrationBuilder.DropTable(
                name: "SEC_RoleClaim");

            migrationBuilder.DropTable(
                name: "SEC_Identity");

            migrationBuilder.DropTable(
                name: "SEC_Role");
        }
    }
}
