using mycooking.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        private ApiService _apiService;
        public Register()
        {
            this.InitializeComponent();
            _apiService = ApiService.GetInstance();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtén los valores de las cajas de texto
            string correo = txtEmail.Text;
            string contrasenya = txtPassword.Password;
            string confirmarContrasenya = txtConfirmPassword.Password;
            string rol = "";

            // Valida que el correo tenga un formato válido
            if (!IsValidEmail(correo))
            {
                txtMessage.Text = "Formato de correo electrónico inválido.";
                return;
            }

            // Valida que las contraseñas coincidan
            if (contrasenya != confirmarContrasenya)
            {
                txtMessage.Text = "Las contraseñas no coinciden.";
                return;
            }

            try
            {
                // Enviar los datos del usuario al servidor para registrar
                var response = await _apiService.Register(correo, contrasenya, rol);

                // El registro fue exitoso, mostrar un mensaje de éxito
                txtMessage.Text = "Registro exitoso. Ahora puedes iniciar sesión.";

                // Después de un registro exitoso, redirigir al usuario a la página de inicio de sesión
                Frame.Navigate(typeof(Login));
            }
            catch (HttpRequestException ex)
            {
                // Manejar el caso en que ocurra un error en la solicitud HTTP
                txtMessage.Text = "Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro tipo de error
                txtMessage.Text = "Error: " + ex.Message;
            }
        }

        private bool IsValidEmail(string email)
        {
            // Expresión regular para validar el formato del correo electrónico
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Verificar si el correo electrónico coincide con el patrón
            return Regex.IsMatch(email, pattern);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Redirigir al usuario a la página de inicio de sesión
            Frame.Navigate(typeof(Login));
        }
    }
}
