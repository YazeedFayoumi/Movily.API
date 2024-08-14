namespace test1.Dto
{
    public class MovieDto
    {
        public required string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
        public required string Description { get; set; }
        public int Duration { get; set; }
    }
}
