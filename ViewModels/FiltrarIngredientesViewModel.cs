using mycooking.Models;
using mycooking.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace mycooking.ViewModels
{
    public class FiltrarIngredientesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Receta> _listaDeRecetas;
        public ObservableCollection<Receta> ListaDeRecetas
        {
            get { return _listaDeRecetas; }
            set
            {
                _listaDeRecetas = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public FiltrarIngredientesViewModel()
        {
            ListaDeRecetas = new ObservableCollection<Receta>();
        }

        public async Task ObtenerRecetasFiltradas(string ingredientes)
        {
            var apiService = ApiService.GetInstance();

            List<Receta> recetasFiltradas = await apiService.ObtenerRecetasPorIngredientes(ingredientes);

            if (recetasFiltradas != null)
            {
                ListaDeRecetas.Clear();
                foreach (var receta in recetasFiltradas)
                {
                    ListaDeRecetas.Add(receta);
                    Debug.WriteLine($"Receta: {receta.Nombre}");
                }
            }
            else
            {
                Debug.WriteLine("No se encontraron recetas con los ingredientes especificados.");
            }
        }
    }
}
