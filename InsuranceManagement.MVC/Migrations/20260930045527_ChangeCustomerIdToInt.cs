using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceManagement.MVC.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCustomerIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Customers",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Customers",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "NewId",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE [Customers] SET [NewId] = CONVERT(nvarchar(450), [Id]);");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Customers",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");
        }
    }
}
