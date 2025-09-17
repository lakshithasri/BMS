using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KlarityLive.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_tenanttenancy_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenancy_Building_BuildingId",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenancy_Tenant_TenantId",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.DropIndex(
                name: "IX_Tenancy_BuildingId_TenantId",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.DropIndex(
                name: "IX_Tenancy_TenantId",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                schema: "BMS",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                schema: "BMS",
                table: "Tenant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                schema: "BMS",
                table: "Tenancy",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "BMS",
                table: "Tenancy",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TenantTenancy",
                schema: "BMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TenancyId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenancyId1 = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantTenancy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantTenancy_Tenancy_TenancyId1",
                        column: x => x.TenancyId1,
                        principalSchema: "BMS",
                        principalTable: "Tenancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantTenancy_Tenant_TenancyId",
                        column: x => x.TenancyId,
                        principalSchema: "BMS",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenancy_BuildingId",
                schema: "BMS",
                table: "Tenancy",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTenancy_TenancyId",
                schema: "BMS",
                table: "TenantTenancy",
                column: "TenancyId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTenancy_TenancyId1",
                schema: "BMS",
                table: "TenantTenancy",
                column: "TenancyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenancy_Building_BuildingId",
                schema: "BMS",
                table: "Tenancy",
                column: "BuildingId",
                principalSchema: "BMS",
                principalTable: "Building",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenancy_Building_BuildingId",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.DropTable(
                name: "TenantTenancy",
                schema: "BMS");

            migrationBuilder.DropIndex(
                name: "IX_Tenancy_BuildingId",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                schema: "BMS",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "BMS",
                table: "Tenancy");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                schema: "BMS",
                table: "Tenant",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                schema: "BMS",
                table: "Tenancy",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                schema: "BMS",
                table: "Tenancy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tenancy_BuildingId_TenantId",
                schema: "BMS",
                table: "Tenancy",
                columns: new[] { "BuildingId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tenancy_TenantId",
                schema: "BMS",
                table: "Tenancy",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenancy_Building_BuildingId",
                schema: "BMS",
                table: "Tenancy",
                column: "BuildingId",
                principalSchema: "BMS",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenancy_Tenant_TenantId",
                schema: "BMS",
                table: "Tenancy",
                column: "TenantId",
                principalSchema: "BMS",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
