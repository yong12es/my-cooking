using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Net.Http.Headers;
using mycooking.Services;
using mycooking.Models;
using Windows.Services.Maps;
using Windows.Media.Protection.PlayReady;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CrearRecetaPage : Page
    {
        
        private StorageFile imagenSeleccionada;

        private ApiService _apiService;

        private RecetaViewModel recetaService;

        

        private int idPaisSeleccionado;
        public CrearRecetaPage()
        {
            this.InitializeComponent();
            _apiService = ApiService.GetInstance();
            recetaService = RecetaViewModel.Instance;

            CargarPaises();

            PaisesComboBox.SelectionChanged += PaisesComboBox_SelectionChanged;
        }

        private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            StackPanel nuevoIngredientePanel = new StackPanel();
            nuevoIngredientePanel.Orientation = Orientation.Horizontal;
            nuevoIngredientePanel.Margin = new Windows.UI.Xaml.Thickness(0, 5, 0, 0);

            TextBox nuevoIngredienteTextBox = new TextBox();
            nuevoIngredienteTextBox.PlaceholderText = "Ingrediente";
            nuevoIngredienteTextBox.Width = 150;

            TextBox nuevaCantidadTextBox = new TextBox();
            nuevaCantidadTextBox.PlaceholderText = "Cantidad";
            nuevaCantidadTextBox.Width = 100;
            nuevaCantidadTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 0, 0, 0);

            nuevoIngredientePanel.Children.Add(nuevoIngredienteTextBox);
            nuevoIngredientePanel.Children.Add(nuevaCantidadTextBox);

           
            IngredientesStackPanel.Children.Add(nuevoIngredientePanel);
        }

        private async void CrearReceta_Click(object sender, RoutedEventArgs e)
        {
            try
            {            
                // Verificar si el token de acceso está vacío
                if (string.IsNullOrEmpty(_apiService.AccessToken))
                {
                    // El token de acceso está vacío, mostrar un mensaje de error
                    Debug.WriteLine("El token de acceso está vacío.");
                    MostrarMensaje("Debe iniciar sesión antes de crear una receta. El token de acceso está vacío.");
                    return;
                }
                else
                {
                    // Mostrar el contenido del token de acceso en la consola de depuración
                    Debug.WriteLine("Contenido del token de acceso: primera linea" + _apiService.AccessToken);
                }

                // Obtener los valores de los ingredientes
                List<Ingrediente> ingredientes = new List<Ingrediente>();
                foreach (StackPanel panelIngrediente in IngredientesStackPanel.Children)
                {
                    TextBox textBoxIngrediente = panelIngrediente.Children[0] as TextBox;
                    TextBox textBoxCantidad = panelIngrediente.Children[1] as TextBox;

                    // Verificar que ambos campos no estén vacíos
                    if (!string.IsNullOrEmpty(textBoxIngrediente.Text) && !string.IsNullOrEmpty(textBoxCantidad.Text))
                    {
                        Ingrediente ingrediente = new Ingrediente
                        {
                            NombreIngre = textBoxIngrediente.Text,
                            Cantidad = textBoxCantidad.Text
                        };
                        ingredientes.Add(ingrediente);
                    }
                }

                string nombre = NombreRecetaTextBox.Text;
                string descripcion = DescripcionTextBox.Text;
                string instrucciones = InstruccionesTextBox.Text;

                if (imagenSeleccionada == null)
                {
                    MostrarMensaje("Debe seleccionar una imagen para la receta.");
                    return;
                }
                byte[] imagenBytes;
                using (IRandomAccessStream stream = await imagenSeleccionada.OpenReadAsync())
                {
                    using (DataReader reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        imagenBytes = new byte[stream.Size];
                        reader.ReadBytes(imagenBytes);
                    }
                }
                Pais paisSeleccionado = PaisesComboBox.SelectedItem as Pais;
                if (paisSeleccionado == null)
                {
                    MostrarMensaje("Debe seleccionar un país para la receta.");
                    return;
                }
                // Crear la receta
                Receta receta = new Receta()
                {
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Instrucciones = instrucciones,
                    Imagen = Convert.ToBase64String(imagenBytes),
                    PaisId = idPaisSeleccionado,
                    Ingredientes = ingredientes

                };
                recetaService.AgregarReceta(receta);

                // Crear la receta usando ApiService
                await _apiService.CrearReceta(receta,"");

                // Mostrar un mensaje de éxito
                MostrarMensaje("Receta creada exitosamente.");
            }
            catch (UnauthorizedAccessException ex)
            {
                MostrarMensaje("No tienes permiso para realizar esta acción.");
                Debug.WriteLine("Error al crear la receta: " + ex.Message);

            }
            catch (HttpRequestException ex)
            {
                MostrarMensaje($"Error al crear la receta: {ex.Message}");
                Debug.WriteLine("Error al crear la receta: " + ex.Message);
            }
            catch (Exception ex)
            {
                
                MostrarMensaje($"Error al crear la receta: {ex.Message}");
            }
        }

        private async void CargarImagen_Click(object sender, RoutedEventArgs e)
        {

            // Configurar el FileOpenPicker
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpg");

            imagenSeleccionada = await filePicker.PickSingleFileAsync();
            if (imagenSeleccionada != null)
            {
                // Abrir el archivo seleccionado y cargar la imagen en el control de imagen
                using (IRandomAccessStream fileStream = await imagenSeleccionada.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    miImagenControl.Source = bitmapImage; 
                }
            }
        }


        private async void CargarPaises()
        {
            List<Pais> paises = await ApiService.GetInstance().ObtenerPaises();

            if (paises != null && paises.Any())
            {
                PaisesComboBox.ItemsSource = paises;
                PaisesComboBox.DisplayMemberPath = "Nombre";
                PaisesComboBox.SelectedValuePath = "Id";
            }
            else
            {
               
            }
        }
        private void PaisesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaisesComboBox.SelectedItem != null)
            {
                Pais paisSeleccionado = PaisesComboBox.SelectedItem as Pais;
                if (paisSeleccionado != null)
                {
                    idPaisSeleccionado = paisSeleccionado.Id;
                    Debug.WriteLine("ID del país seleccionado: " + idPaisSeleccionado);
                }
            }
        }



        private void MostrarMensaje(string mensaje)
        {
            
            var dialog = new Windows.UI.Popups.MessageDialog(mensaje);
            _ = dialog.ShowAsync();
        }

       
    }
}
