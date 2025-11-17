namespace Mosaico.Api.Dtos
{
    public class MissionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

        // Aqui mantemos string para facilitar consumo pela API
        public string Type { get; set; } = default!; // "daily" / "weekly"

        public int RewardXp { get; set; }
    }
}
