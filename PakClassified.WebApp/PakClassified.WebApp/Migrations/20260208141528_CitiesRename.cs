using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace a._PakClassified.WebApp.Entities.Migrations
{
    /// <inheritdoc />
    public partial class CitiesRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CIties_Provinces_ProvinceId",
                table: "CIties");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAreas_CIties_CityId",
                table: "CityAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CIties",
                table: "CIties");

            migrationBuilder.RenameTable(
                name: "CIties",
                newName: "Cities");

            migrationBuilder.RenameIndex(
                name: "IX_CIties_ProvinceId",
                table: "Cities",
                newName: "IX_Cities_ProvinceId");

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "AdvertisementImages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityAreas_Cities_CityId",
                table: "CityAreas",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAreas_Cities_CityId",
                table: "CityAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "CIties");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ProvinceId",
                table: "CIties",
                newName: "IX_CIties_ProvinceId");

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "AdvertisementImages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CIties",
                table: "CIties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CIties_Provinces_ProvinceId",
                table: "CIties",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityAreas_CIties_CityId",
                table: "CityAreas",
                column: "CityId",
                principalTable: "CIties",
                principalColumn: "Id");
        }
    }
}
