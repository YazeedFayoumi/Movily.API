using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test1.Migrations
{
    /// <inheritdoc />
    public partial class AddMockCutomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Customer (Name, Email, Password, MembershipTypeId) VALUES('Ahmad', 'npc1@gmail.com', '1234', '1')");
            migrationBuilder.Sql("INSERT INTO Customer (Name, Email, Password, MembershipTypeId) VALUES('Khaled', 'npc2@gmail.com', '1234', '2')");
            migrationBuilder.Sql("INSERT INTO Customer (Name, Email, Password, MembershipTypeId) VALUES('Yazeed', 'npc3@gmail.com', '1234', '1')");
            migrationBuilder.Sql("INSERT INTO Customer (Name, Email, Password, MembershipTypeId) VALUES('Ahmad', 'npc4@gmail.com', '1234', '3')");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
