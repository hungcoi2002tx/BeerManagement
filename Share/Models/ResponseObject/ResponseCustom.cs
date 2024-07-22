using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.ResponseObject
{
    public class ResponseCustom<T> where T : class
    {
        public bool Status { get; set; }
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Object { get; set; }

        public List<T>? Objects { get; set; }
        public int Total { get; set; }
    }
}
