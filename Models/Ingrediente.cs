using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycooking.Models
{
    public class Ingrediente
    {
        [JsonProperty("cantidad")] 
        public string Cantidad { get; set; }
        [JsonProperty("nombre")]
        public string NombreIngre { get; set; }
    }
}
