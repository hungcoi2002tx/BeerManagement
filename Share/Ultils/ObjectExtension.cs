using Share.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Ultils
{
    public static class ObjectExtension
    {
        public static string GetMesssageError(this string message, string location = null)
        {
            return $"Internal server error: {message} in {location}";
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return str != "" && str != null;
        }

        public static string GenerateUrl(this Page page, int number)
        {
            return page.BaseUrl + "?pageIndex=" + number;
        }
    }
}
