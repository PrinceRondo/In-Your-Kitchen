using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InYourFridge.Migrations
{
    public partial class KitchenProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KitchenProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPreparation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthBenefit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    EntryTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitchenProducts_AspNetUsers_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KitchenProducts_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KitchenProducts_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KitchenProducts_EntryTypes_EntryTypeId",
                        column: x => x.EntryTypeId,
                        principalTable: "EntryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitchenProducts_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitchenProductItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenProductItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitchenProductItems_KitchenItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "KitchenItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitchenProductItems_KitchenProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "KitchenProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenProductItems_ItemId",
                table: "KitchenProductItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenProductItems_ProductId",
                table: "KitchenProductItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenProducts_ApprovedBy",
                table: "KitchenProducts",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenProducts_CreatedBy",
                table: "KitchenProducts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenProducts_EntryTypeId",
                table: "KitchenProducts",
                column: "EntryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenProducts_StatusId",
                table: "KitchenProducts",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenProducts_UpdatedBy",
                table: "KitchenProducts",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KitchenProductItems");

            migrationBuilder.DropTable(
                name: "KitchenProducts");
        }
    }
}
