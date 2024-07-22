using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Share.Models.ResponseObject;

namespace Share.Ultils
{
    public static class ResponeExtentions<T> where T : class
    {
        #region Yêu cầu từ client không hợp lệ, thiếu hoặc sai dữ liệu 400
        public static ResponseCustom<T> GetError400(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 400,
                Status = false,
            };
        }
        #endregion

        #region Không có quyền truy cập hoặc xác thực không hợp lệ 401.
        public static ResponseCustom<T> GetError401(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 401,
                Status = false,
            };
        }
        #endregion

        #region Không có quyền truy cập vào tài nguyên 403.
        public static ResponseCustom<T> GetError403(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 403,
                Status = false,
            };
        }
        #endregion

        #region Không tìm thấy tài nguyên được yêu cầu 404.
        public static ResponseCustom<T> GetError404(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 404,
                Status = false,
            };
        }
        #endregion

        #region #region Không có quyền truy cập hoặc xác thực không hợp lệ 405.
        public static ResponseCustom<T> GetError405(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 405,
                Status = false,
            };
        }
        #endregion

        #region Có xung đột với tài nguyên hiện tại 409.
        public static ResponseCustom<T> GetError409(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 409,
                Status = false,
            };
        }
        #endregion

        #region Chức năng yêu cầu chưa được triển khai 501.
        public static ResponseCustom<T> GetError501(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 501,
                Status = false,
            };
        }
        #endregion

        #region Dịch vụ hiện không sẵn sàng để xử lý yêu cầu 503.
        public static ResponseCustom<T> GetError503(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 503,
                Status = false,
            };
        }
        #endregion

        #region Lỗi không xác định trên server 500.
        public static ResponseCustom<T> GetError500(string mess)
        {
            return new ResponseCustom<T>()
            {
                Message = mess,
                StatusCode = 500,
                Status = false,
            };
        }
        #endregion
    }
}
