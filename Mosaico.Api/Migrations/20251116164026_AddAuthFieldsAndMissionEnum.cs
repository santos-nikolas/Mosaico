using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mosaico.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthFieldsAndMissionEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Primeiro, convertemos os valores textuais para valores numéricos em texto
            migrationBuilder.Sql("UPDATE [Missions] SET [Type] = '1' WHERE [Type] = 'daily';");
            migrationBuilder.Sql("UPDATE [Missions] SET [Type] = '2' WHERE [Type] = 'weekly';");
            migrationBuilder.Sql("UPDATE [Missions] SET [Type] = '1' WHERE [Type] NOT IN ('1', '2');");

            // Depois alteramos de nvarchar para int
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Missions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Missions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
