using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.ServiceModel;
using mycooking.Models;
using mycooking.Services;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using System.ComponentModel;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class RecetasMundoPag : Page
    {
        
        
        public RecetasMundoPag()
        {
            
            this.InitializeComponent();
            DataContext = RecetaPorPaisViewModel.Instance;
            CargarPaises();
        }

        private async void CargarPaises()
        {
            RecetaPorPaisViewModel.Instance.Paises.Clear();
            // Obtener la lista de países desde el servicio de la API
            var apiService = ApiService.GetInstance();
            var paises = await apiService.ObtenerPaises();
            var image = Image.SourceProperty;
            if (paises != null)
            {
                foreach (var pais in paises)
                {
                    RecetaPorPaisViewModel.Instance.AgregarPais(pais);
                }
                    
            }
        }

        private async void CountryButton_Click(object sender, RoutedEventArgs e)
        {
            RecetaPorPaisViewModel.Instance.Recetas.Clear();
            try
            {
                var apiService = ApiService.GetInstance();
                // Obtener el botón que se ha hecho clic
                Button clickedButton = (Button)sender; 
               
                // Obtener el país seleccionado del DataContext del botón
                Pais selectedCountry = (Pais)clickedButton.DataContext;

                // Obtener las recetas asociadas con el país seleccionado
                List<Receta> recetas = await apiService.ObtenerRecetasPorPais(selectedCountry.Id);
                

                if (recetas != null && recetas.Count >0) 
                {
                    foreach (var receta in recetas)
                    {
                        RecetaPorPaisViewModel.Instance.AgregarReceta(receta);
                        
                    }

                }
                else
                {
                    Debug.WriteLine("No se encontro recetas");
                }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al obtener las recetas: " + ex.Message);
               
            }
        }

        private Receta _selectedRecipe;
        public Receta SelectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }

        private bool _isRecipeSelected;
        public bool IsRecipeSelected
        {
            get { return _isRecipeSelected; }
            set
            {
                _isRecipeSelected = value;
                OnPropertyChanged(nameof(IsRecipeSelected));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
         
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Receta_Click(object sender, ItemClickEventArgs e)
        {
            Receta selectedRecipe = (Receta)e.ClickedItem;
            SelectedRecipe = selectedRecipe;
            IsRecipeSelected = true;
        }
    }
}
