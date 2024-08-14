namespace test1.Dto
{
    public class CustomerDtoSignIn
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public required int MembershipType { get; set; }
    }
}
