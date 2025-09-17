namespace HaloStats.Domain.Entities
{
    public class HaloPostGameReport
    {
        public required Guid Id { get; set; }
        public required Guid GameUniqueId { get; set; }
        public string GamerTag { get; set; }
        public required string Report { get; set; }

    }
}
