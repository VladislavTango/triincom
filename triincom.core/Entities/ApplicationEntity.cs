using triincom.Core.Enums;

namespace triincom.Core.Entities
{
    public class ApplicationEntity
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public int TermValue { get; set; }
        public decimal InterestValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
