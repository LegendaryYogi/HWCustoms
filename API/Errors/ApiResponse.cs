namespace API.Errors
{
    public class ApiResponse            //50
    {
        public ApiResponse(int statusCode, string message = null)
        {
            Statuscode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);    // if message is null execute the other side
        }

        public int Statuscode { get; set; }

        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch            //new switch expression from c# version 8
            {
                400 => "A bad request, you have made",      // status code we are looking for
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
                _ => null           //default case
            };
        }
    }
}