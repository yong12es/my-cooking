using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using mycooking.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Storage;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Storage;


namespace mycooking.ViewModels
{
    public class AgregarIngredientesViewModel : BaseViewModel
    {
        private string _nuevoNombre;
        private string _nuevaCantidad;

        public string NuevoNombre
        {
            get => _nuevoNombre;
            set => SetProperty(ref _nuevoNombre, value);
        }

        public string NuevaCantidad
        {
            get => _nuevaCantidad;
            set => SetProperty(ref _nuevaCantidad, value);
        }

        public ObservableCollection<Ingrediente> Ingredientes { get; set; }

        public ICommand AgregarIngredienteCommand { get; }

        public AgregarIngredientesViewModel()
        {
            Ingredientes = new ObservableCollection<Ingrediente>();
            AgregarIngredienteCommand = new RelayCommand(AgregarIngrediente);
        }

        private void AgregarIngrediente()
        {
            if (!string.IsNullOrEmpty(NuevoNombre) && !string.IsNullOrEmpty(NuevaCantidad))
            {
                var nuevoIngrediente = new Ingrediente { NombreIngre = NuevoNombre, Cantidad = NuevaCantidad };
                Ingredientes.Add(nuevoIngrediente);
                NuevoNombre = string.Empty;
                NuevaCantidad = string.Empty;
            }
        }
    }

    public class Ingrediente
    {
        public string NombreIngre { get; set; }
        public string Cantidad { get; set; }
    }
}
