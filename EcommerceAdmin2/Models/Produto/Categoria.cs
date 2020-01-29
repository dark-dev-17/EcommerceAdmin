using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Produto
{
    public class Categoria
    {
        public int Id { get;  set; }
        public string Description { get;  set; }
    }
}
