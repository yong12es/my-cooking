using mycooking.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using mycooking.Models;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CrearTallerPage : Page
    {
        private TallerService _tallerService;
        private ApiService _apiService;
        public CrearTallerPage()
        {
            this.InitializeComponent();
            _tallerService = TallerService.Instance;
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

                // Mostrar un mensaje de éxito
                MostrarMensaje("Taller creado exitosamente.");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al crear el taller: {ex.Message}");
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
