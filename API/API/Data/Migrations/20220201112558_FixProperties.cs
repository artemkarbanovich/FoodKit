using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class FixProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Sets",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dishes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Sets",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "TotalPrice",
                table: "Orders",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Dishes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
