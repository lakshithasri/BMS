using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KlarityLive.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_tenantId_column_to_user_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                schema: "Admin",
                table: "User",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Admin",
                table: "User");
        }
    }
}
