using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Constant
{
    public class RestApiName
    {
        public const string BASEURL = "https://localhost:7169/api/";

        public const string POST_ALL_LIST_CATEGORY = BASEURL + "Category/GetAll";
        public const string POST_PAGE_LIST_CATEGORY = BASEURL + "Category/GetPage";
        public const string POST_Add_CATEGORY = BASEURL + "Category/Add";
        public const string DELETE_CATEGORY = BASEURL + "Category/Delete/{0}";
        public const string PUT_CATEGORY = BASEURL + "Category/Update";

        public const string POST_ALL_LIST_SUPPLIER = BASEURL + "Supplier/GetAll";
        public const string POST_PAGE_LIST_SUPPLIER = BASEURL + "Supplier/GetPage";
        public const string POST_Add_SUPPLIER = BASEURL + "Supplier/Add";
        public const string DELETE_SUPPLIER = BASEURL + "Supplier/Delete/{0}";
        public const string PUT_SUPPLIER = BASEURL + "Supplier/Update";

        public const string GET_ALL_LIST_PRODUCT = BASEURL + "Product/GetAll";
        public const string POST_PAGE_LIST_PRODUCT = BASEURL + "Product/GetPage";
        public const string POST_Add_PRODUCT = BASEURL + "Product/Add";
        public const string DELETE_PRODUCT = BASEURL + "Product/Delete/{0}";
        public const string PUT_PRODUCT = BASEURL + "Product/Update";

        public const string GET_LIST_TABLE = BASEURL + "Table/GetAll";
        public const string GET_LIST_TABLE_BY_CONDITION = BASEURL + "Table/GetPage";
        public const string DELETE_TABLE = BASEURL + "Table/Delete/{0}";
        public const string UPDATE_TABLE = BASEURL + "Table/Update";
        public const string ADD_TABLE = BASEURL + "Table/Add";

    }
}
