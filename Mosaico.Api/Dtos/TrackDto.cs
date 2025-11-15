namespace Mosaico.Api.Dtos
{
    public class TrackDto
    {
        public int Id { get; set; }              // usado em respostas
        public string Title { get; set; } = default!;
        public string Area { get; set; } = default!;
        public int TotalLessons { get; set; }
        public int EstimatedHours { get; set; }
    }
}
