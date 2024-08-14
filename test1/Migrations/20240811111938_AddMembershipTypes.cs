using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test1.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO MembershipType (SignUpFee, Duration, Discount) VALUES(0, 0, 0)");
            migrationBuilder.Sql("INSERT INTO MembershipType (SignUpFee, Duration, Discount) VALUES(5, 1, 10)");
            migrationBuilder.Sql("INSERT INTO MembershipType (SignUpFee, Duration, Discount) VALUES(10, 2, 20)");
            migrationBuilder.Sql("INSERT INTO MembershipType (SignUpFee, Duration, Discount) VALUES(20, 3, 35)");
            migrationBuilder.Sql("INSERT INTO MembershipType (SignUpFee, Duration, Discount) VALUES(60, 12, 40)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
