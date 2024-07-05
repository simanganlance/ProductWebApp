namespace ProductApi.DTOs
{
    /// <summary>
    /// Represents Error Response.
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Error response status code.
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Error message response.
        /// </summary>
        public string Message { get; set; }
    }
}
