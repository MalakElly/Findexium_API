
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{

 public class CurvePoint
    {
        [Key]
        public int Id { get; set; }

        public int? CurveId { get; set; }
        public DateTime? AsOfDate { get; set; }
        public double? Term { get; set; }
        public double? Value { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}