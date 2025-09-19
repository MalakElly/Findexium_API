using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public enum TradeType
    {
        BUY,
        SELL
    }

    public class Trade
    {
        [Key]
        public int TradeId { get; set; }

        [Required, MaxLength(100)]
        public string Account { get; set; } = string.Empty;


        [Required]
        public TradeType Type { get; set; } = TradeType.BUY; // valeur par défaut : BUY

        [Required, Range(1, double.MaxValue, ErrorMessage = "Quantity must be positive")]
        public double BuyQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public double? SellQuantity { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public double BuyPrice { get; set; }

        [Range(0, double.MaxValue)]
        public double? SellPrice { get; set; }

        public DateTime? TradeDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? Security { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(100)]
        public string? Trader { get; set; }

        [MaxLength(50)]
        public string? Benchmark { get; set; }

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
