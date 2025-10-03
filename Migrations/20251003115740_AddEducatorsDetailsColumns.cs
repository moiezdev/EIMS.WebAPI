using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EIMS.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEducatorsDetailsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdmissionDate",
                table: "Educators",
                newName: "DateJoined");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateJoined",
                table: "Educators",
                newName: "AdmissionDate");
        }
    }
}
