using System.ComponentModel.DataAnnotations;

namespace asp.net_workshop_real_app_public.Models
{
    public class NewBookModel
    {
        [Required(ErrorMessage = "Please add a title")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Please add the author id")]
        public int AuthorId { get; set; }
        public string BookCoverPath { get; set; }

        public double Price { get; set; }
    }
}
