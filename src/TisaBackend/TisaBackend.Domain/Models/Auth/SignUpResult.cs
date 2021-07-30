namespace TisaBackend.Domain.Models.Auth
{
    public class SignUpResult
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }

        public SignUpResult()
        {
            
        }

        public SignUpResult(int statusCode, string status, string message)
        {
            StatusCode = statusCode;
            Status = status;
            Message = message;
        }
    }
}