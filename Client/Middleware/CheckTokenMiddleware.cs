using Share.Constant;

namespace Client.Middleware
{
    public class CheckTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lấy token từ session
            var token = context.Session.GetString("JWToken");

            // Kiểm tra token
            if (string.IsNullOrEmpty(token))
            {
                // Chuyển hướng về trang login
                context.Response.Redirect(GlobalVariants.LINK_LOGIN);
                return;
            }

            // Tiếp tục xử lý yêu cầu
            await _next(context);
        }
    }
}
