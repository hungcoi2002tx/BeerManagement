using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Constant
{
    public class ExecuteRespone<T> where T : class
    {
        public bool Status { get; set; }
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Object { get; set; }   
    }
}
