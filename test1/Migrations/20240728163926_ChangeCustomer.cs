using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test1.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_MembershipType_MembershipTypeId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerMovie");

            migrationBuilder.DropIndex(
                name: "IX_Customer_MembershipTypeId",
                table: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MovieId",
                table: "Customer",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Movie_MovieId",
                table: "Customer",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Movie_MovieId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_MovieId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Customer");

            migrationBuilder.CreateTable(
                name: "CustomerMovie",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMovie", x => new { x.CustomerId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_CustomerMovie_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerMovie_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MembershipTypeId",
                table: "Customer",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMovie_MovieId",
                table: "CustomerMovie",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_MembershipType_MembershipTypeId",
                table: "Customer",
                column: "MembershipTypeId",
                principalTable: "MembershipType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
