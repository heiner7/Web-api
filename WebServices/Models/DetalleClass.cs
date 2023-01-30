using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.Models
{
    public class DetalleClass
    {
        public int id { get; set; }
        public int id_articulo { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }

    }
}