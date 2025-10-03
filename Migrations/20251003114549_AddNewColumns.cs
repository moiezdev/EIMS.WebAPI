using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EIMS.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Educators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Educators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Educators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Educators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                table: "Educators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Educators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Educators",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Educators");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Educators");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Educators");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Educators");

            migrationBuilder.DropColumn(
                name: "Qualification",
                table: "Educators");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Educators");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Educators");
        }
    }
}
