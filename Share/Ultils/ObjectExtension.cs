using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Share.Constant;
using Share.Models.Domain;
using Share.Models.PagingObject;

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

        public static string GenerateGuid(this string str)
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetRoleString(this int role)
        {
            switch (role)
            {
                case 1:
                    return "Admin";
                    break;
                case 2:
                    return "Staff";
                    break;
                case 3:
                    return "Manager";
                    break;
                default:
                    return null;
            }
        }

        public static string CheckValidRequestExtention(this HttpResponseMessage res)
        {
            if(res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return GlobalVariants.PAGE_403;
            }
            if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return GlobalVariants.PAGE_503;
            }
            return null;
        }
    }
}
