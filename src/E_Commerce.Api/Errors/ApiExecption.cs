namespace E_Commerce.Api.Errors
{
    public class ApiExecption:BaseCommonResponse
    {
        public ApiExecption(int statusCode , string message=null,string details=null) : base(statusCode,message)
        {
                Details= details;
        }
        public string Details {  get; set; }
    }
}
