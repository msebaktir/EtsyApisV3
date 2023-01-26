namespace dotnetEtsyApp.Models
{
    public class ReturnData<T>
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public T Data { get; set; } = default(T);

      

        public ReturnData(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public ReturnData(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ReturnData(T data)
        {
            Success = true;
            Message = "";
            Data = data;
        }
    }
}