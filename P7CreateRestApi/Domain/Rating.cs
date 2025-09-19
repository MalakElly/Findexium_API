using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers.Domain
{

    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        public string? MoodysRating { get; set; }

        [MaxLength(10)]
        public string? SandPRating { get; set; }

        [MaxLength(10)]
        public string? FitchRating { get; set; }

        public byte? OrderNumber { get; set; }
    }
}