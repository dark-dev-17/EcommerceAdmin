using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Sistema
{
    public class DBException : Exception
    {
        public DBException()
        {

        }

        public DBException(string mensaje)
            : base(mensaje)
        {

        }

    }
}
