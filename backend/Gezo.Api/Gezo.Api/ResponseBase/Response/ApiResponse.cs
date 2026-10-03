namespace Gezo.Api.ResponseBase.Response
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();

        // success case
        public ApiResponse(T data ,string msg,int statusCode)
        {
            Data = data;
            Message = msg;
            StatusCode = statusCode;
            IsSuccess = true;

        }

        // fail case

        public ApiResponse(string msg, int statusCode,List<string>? errors =null)
        {
            Errors = errors;
            Message = msg;
            StatusCode = statusCode;
            IsSuccess = false;

        }

    }
}
