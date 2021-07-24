namespace TisaBackend.Domain.Models.Auth
{
    public class SignInResult
    {
        public int StatusCode { get; set; }
        public SignInDetails SignInDetails { get; set; }

        public SignInResult()
        {
            
        }

        public SignInResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public SignInResult(int statusCode, SignInDetails signInDetails)
        {
            StatusCode = statusCode;
            SignInDetails = signInDetails;
        }
    }
}