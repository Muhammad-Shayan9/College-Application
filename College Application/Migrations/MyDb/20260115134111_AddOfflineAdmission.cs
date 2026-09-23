using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace College_Application.Migrations.MyDb
{
    /// <inheritdoc />
    public partial class AddOfflineAdmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOfflineSubmission",
                table: "AdmissonDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UploadedPdfPath",
                table: "AdmissonDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOfflineSubmission",
                table: "AdmissonDetails");

            migrationBuilder.DropColumn(
                name: "UploadedPdfPath",
                table: "AdmissonDetails");
        }
    }
}
