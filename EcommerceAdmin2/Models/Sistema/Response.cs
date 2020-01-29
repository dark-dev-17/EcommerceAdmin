using EcommerceAdmin2.Models.Documents;
using EcommerceAdmin2.Models.BussinesPartner;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Sistema
{
    public class BP_Documents : ActionResult
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public DocumentGeneral DocumentGeneral { get; set; }
    }
    public class Response<T> : ActionResult
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public T Objeto { get; set; }
    }
    public class ResponseList<T> : ActionResult
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public List<T> Records { get; set; }
        public int Count { get { return Records.Count; } }
    }
    public class Response : ActionResult
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
    }
}
