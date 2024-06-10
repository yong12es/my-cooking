using System;
using System.Collections.Generic;

namespace mycooking.Models
{
    public class ApiResponse
    {
        public bool Ok { get; set; }
        public List<Taller> Talleres { get; set; }
    }

    public class Taller
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
        public int duracion { get; set; }
        public string descripcion { get; set; }
        public int? UsuarioId { get; set; }
    }
}
