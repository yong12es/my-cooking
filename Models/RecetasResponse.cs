using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace mycooking.Models
{
    public class RecetasResponse
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("recetas")]
        public List<Receta> Recetas { get; set; }


    }

   

}
