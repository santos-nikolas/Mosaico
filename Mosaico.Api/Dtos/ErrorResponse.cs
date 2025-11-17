namespace Mosaico.Api.Common
{
    public class ErrorResponse
    {
        public string TraceId { get; set; } = default!;
        public int StatusCode { get; set; }
        public string Message { get; set; } = default!;
        public string? Details { get; set; }
    }
}
