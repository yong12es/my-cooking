using mycooking.Models;
using mycooking.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CrearTallerPage : Page
    {
        private TallerViewModel _tallerService;
        private ApiService _apiService;
        public CrearTallerPage()
        {
            this.InitializeComponent();
            _tallerService = TallerViewModel.Instance;
            _apiService = ApiService.GetInstance();

        }

        private async void CrearTaller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = NombreTallerTextBox.Text;
                DateTime fecha = FechaDatePicker.Date.DateTime;
                DateTime fechaSinHora = fecha.Date;
                int duracionn = int.Parse(DuracionTextBox.Text);
                string descripcion = DescripcionTextBox.Text;

                if (fechaSinHora < DateTime.Now.Date)
                {
                    MostrarMensaje("La fecha del taller no puede ser anterior a la fecha actual.");
                    return;
                }
                // Crear el taller
                Taller taller = new Taller
                {
                    nombre = nombre,
                    fecha = fechaSinHora,
                    duracion = duracionn,
                    descripcion = descripcion
                };

                _tallerService.AgregarTaller(taller);

                await _apiService.CrearTaller(taller, "");


                MostrarMensaje("Taller creado exitosamente.");
            }
            catch (UnauthorizedAccessException ex)
            {
                MostrarMensaje("No tienes permiso para realizar esta acción.");
                Debug.WriteLine("Error al crear el taller: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                MostrarMensaje($"Error al crear el taller: {ex.Message}");
                Debug.WriteLine("Error al crear el taller: " + ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al crear el taller: {ex.Message}");
                Debug.WriteLine("Error al crear el taller: " + ex.Message);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {


            base.OnNavigatingFrom(e);
        }

        private async void MostrarMensaje(string mensaje)
        {
            var dialog = new MessageDialog(mensaje);
            await dialog.ShowAsync();
        }
    }
}
