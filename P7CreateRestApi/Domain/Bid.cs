using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{

    public class Bid
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Account { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }

        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? BidPrice { get; set; }
        public double? AskPrice { get; set; }

        [MaxLength(50)]
        public string? Benchmark { get; set; }

        public DateTime? Date { get; set; }

        [MaxLength(1000)]
        public string? Commentary { get; set; }

        [MaxLength(100)]
        public string? Security { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(100)]
        public string? Trader { get; set; }

        [MaxLength(100)]
        public string? Book { get; set; }

        [MaxLength(100)]
        public string? CreationName { get; set; }
        public DateTime? CreationDate { get; set; }

        [MaxLength(100)]
        public string? RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }

        [MaxLength(100)]
        public string? DealName { get; set; }

        [MaxLength(50)]
        public string? DealType { get; set; }

        [MaxLength(100)]
        public string? SourceListId { get; set; }

        [MaxLength(10)]
        public string? Side { get; set; }
    }
}