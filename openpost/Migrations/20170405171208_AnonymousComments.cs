using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace openpost.Migrations
{
    public partial class AnonymousComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowAnonymousComments",
                table: "Pages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Authors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "Authors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowAnonymousComments",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "Authors");
        }
    }
}
