using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mycooking.Models;

namespace mycooking.Services
{
    public class RecetaViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private static RecetaViewModel _instance;
        public static RecetaViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RecetaViewModel();
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