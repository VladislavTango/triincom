using triincom.Core.Enums;

namespace triincom.Core.DTO
{
    public class ApplicationFilterDto
    {
        public Status? Status { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public int? MinTerm { get; set; }
        public int? MaxTerm { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
