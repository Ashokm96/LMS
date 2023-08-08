namespace LMS.Api.Models
{
    public class Result<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get { return string.IsNullOrEmpty(ErrorMessage) ? true : false; } }
    }
}
