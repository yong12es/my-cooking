using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycooking.Models
{
    public class PaisesResponse
    {
        public bool Ok { get; set; }
        public List<Pais> Pais { get; set; }
    }
}
