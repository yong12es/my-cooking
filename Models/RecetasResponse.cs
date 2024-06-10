
/* Cambio no fusionado mediante combinación del proyecto 'Models'
Antes:
using System;
Después:
using Newtonsoft.Json;
using System;
*/
using Newtonsoft.Json;
using System.Collections.Generic;
/* Cambio no fusionado mediante combinación del proyecto 'Models'
Antes:
using System.Threading.Tasks;
using Newtonsoft.Json;
Después:
using System.Threading.Tasks;
*/


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
