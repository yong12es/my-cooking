using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycooking.Models
{
    public class Receta
    {

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("instrucciones")]
        public string Instrucciones { get; set; }
        [JsonProperty("imagen")]
        public string Imagen { get; set; }
        [JsonProperty("id_pais")]
        public int PaisId { get; set; }
        [JsonProperty("ingredientes")]
        public List<Ingrediente> Ingredientes { get; set; }
    }


}
