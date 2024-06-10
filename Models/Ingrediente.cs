using Newtonsoft.Json;

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
