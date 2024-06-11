﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contact.Domain.Migrations
{
    /// <inheritdoc />
    public partial class modified_user_model_Image_control : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                table: "PersonalInformations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                table: "PersonalInformations");
        }
    }
}
