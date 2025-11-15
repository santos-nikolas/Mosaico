namespace Mosaico.Api.Dtos
{
    public class MissionDto
    {
        public int Id { get; set; }              // usado em respostas
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Type { get; set; } = default!; // daily / weekly
        public int RewardXp { get; set; }
    }
}
