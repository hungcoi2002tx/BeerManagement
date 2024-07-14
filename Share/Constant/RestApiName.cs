using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Constant
{
    public class RestApiName
    {
        public const string BaseUrl = "https://localhost:7169/api/";

        public const string GET_ALL_LIST_CATEGORY = BaseUrl + "Category/GetAll";
        public const string POST_PAGE_LIST_CATEGORY = BaseUrl + "Category/GetPage";
        public const string POST_Add_CATEGORY = BaseUrl + "Category/Add";
        public const string DELETE_CATEGORY = BaseUrl + "Category/Delete/{0}";
        public const string PUT_CATEGORY = BaseUrl + "Category/Update";

        public const string GET_ALL_LIST_SUPPLIER = BaseUrl + "Supplier/GetAll";
        public const string POST_PAGE_LIST_SUPPLIER = BaseUrl + "Supplier/GetPage";
        public const string POST_Add_SUPPLIER = BaseUrl + "Supplier/Add";
        public const string DELETE_SUPPLIER = BaseUrl + "Supplier/Delete/{0}";
        public const string PUT_SUPPLIER = BaseUrl + "Supplier/Update";

        public const string GET_ALL_LIST_PRODUCT = BaseUrl + "Product/GetAll";
        public const string POST_PAGE_LIST_PRODUCT = BaseUrl + "Product/GetPage";
        public const string POST_Add_PRODUCT = BaseUrl + "Product/Add";
        public const string DELETE_PRODUCT = BaseUrl + "Product/Delete/{0}";
        public const string PUT_PRODUCT = BaseUrl + "Product/Update";

    }
}
