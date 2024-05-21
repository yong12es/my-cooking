using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mycooking.Models;

namespace mycooking.Services
{
    public class RecetaService
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private static RecetaService _instance;
        public static RecetaService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RecetaService();
                }
                return _instance;
            }
        }
        private List<Receta> _recetas = new List<Receta>();
        public List<Receta> Recetas
        {
            get { return _recetas; }
            set { _recetas = value; }
        }

        public void AgregarReceta(Receta receta)
        {
            _recetas.Add(receta);
        }

    }
}