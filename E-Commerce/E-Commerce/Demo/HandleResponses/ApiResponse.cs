namespace Demo.HandleResponses
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode,  string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public int StatusCode { get; set; }
        
        public string Message { get; set; }

        private  string GetDefaultMessageForStatusCode(int code)
        
            => code switch
            {
                400 => "BadRequest",
                401 => "You are not authorized",
                402 => "Resourse not found",
                500 => "Internal Server Error",
                _ => null
            };
        
    }
}
