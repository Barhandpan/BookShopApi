namespace asp.net_workshop_real_app_public.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AuthorModel Author { get; set; }
        public double Price { get; set; }
        public string BookCoverPath { get; set; }
    }
}
