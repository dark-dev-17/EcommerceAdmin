using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin1.Models.Sistema
{
    public class Response<T>
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public T Objeto { get; set; }
    }
    public class Response
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
    }
}
