namespace Portfolio.Extension
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext context) 
            {
                return context.Items["userId"] as int? ?? 0;
            }
    }
}
