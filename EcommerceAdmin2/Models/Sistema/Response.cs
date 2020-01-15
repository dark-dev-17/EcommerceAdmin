using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Sistema
{
    public class Response<T>
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public T Objeto { get; set; }
    }
    public class ResponseList<T>
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public List<T> Records { get; set; }
    }
    [Newtonsoft.Json.JsonObject(Title = "Response")]
    public class Response
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Code")]
        public int Code { get; set; }
    }
}
