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

        public const string GET_LIST_CATEGORY = BaseUrl + "Category";

        public const string GET_ALL_LIST_SUPPLIER = BaseUrl + "Supplier/GetAll";
        public const string POST_PAGE_LIST_SUPPLIER = BaseUrl + "Supplier/GetPage";
        public const string POST_Add_SUPPLIER = BaseUrl + "Supplier/Add";
        public const string DELETE_SUPPLIER = BaseUrl + "Supplier/Delete/{0}";
        public const string PUT_SUPPLIER = BaseUrl + "Supplier/Update";

    }
}
