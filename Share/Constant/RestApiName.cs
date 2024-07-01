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

        public const string GET_LIST_SUPPLIER = BaseUrl + "Supplier/GetAll";
        public const string Add_SUPPLIER = BaseUrl + "Supplier/Add";

    }
}
