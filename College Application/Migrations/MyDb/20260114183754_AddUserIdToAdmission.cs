using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace College_Application.Migrations.MyDb
{
    /// <inheritdoc />
    public partial class AddUserIdToAdmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AdmissonDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AdmissonDetails");
        }
    }
}
