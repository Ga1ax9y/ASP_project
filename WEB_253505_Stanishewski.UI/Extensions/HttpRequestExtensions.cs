namespace WEB_253505_Stanishewski.UI.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            // Проверяем заголовок "X-Requested-With"
            return request.Headers["x-requested-with"].ToString().ToLower().Equals("xmlhttprequest");
        }
    }
}
