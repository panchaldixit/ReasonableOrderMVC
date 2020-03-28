using Microsoft.EntityFrameworkCore.Migrations;

namespace ReasonableOrderMVC.Migrations
{
    public partial class AddOrderToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: true),
                    Total = table.Column<int>(nullable: true),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total_Order_Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    OrderId =table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Price", "OrderId" },
                values: new object[] { "Hammer", 6, 0 });
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] {  "Name", "Price", "OrderId" },
                values: new object[] { "Screws", 2, 0 });
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Price", "OrderId" },
                values: new object[] { "ScrewDriver", 5, 0 });
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Price", "OrderId" },
                values: new object[] { "Bolts", 5 , 0});

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
            migrationBuilder.DropTable(
                name: "Sales");
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}