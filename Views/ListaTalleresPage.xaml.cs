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
using mycooking.Models;
using mycooking.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Popups;


// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ListaTalleresPage : Page
    {
        public ListaTalleresPage()
        {
            this.InitializeComponent();
            this.DataContext = TallerViewModel.Instance;
  
            ApiService apiService = ApiService.GetInstance();
            
            CargarTalleresDesdeBD(apiService);
        }

        private async void CargarTalleresDesdeBD(ApiService apiService)
        {
            TallerViewModel.Instance.Talleres.Clear();
            List<Taller> talleres = await apiService.ObtenerTalleresDesdeBD();

            if (talleres != null)
            {
                foreach(var taller in talleres)
                {
                    TallerViewModel.Instance.AgregarTaller(taller);
                }
            }
        }

        private void Participar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var taller = button.DataContext as Taller;
            
            TallerViewModel.Instance.MoverTallerParticipado(taller);

            MostrarMensaje($"Has participado en el taller: {taller.nombre}");
        }

        private async void MostrarMensaje(string mensaje)
        {
            var dialog = new MessageDialog(mensaje);
            await dialog.ShowAsync();
        }
    }
}
