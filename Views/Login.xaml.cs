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

using System.Threading.Tasks;
using Newtonsoft.Json;
using mycooking.Services;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Windows.Services.Maps;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private ApiService _apiService;
        public Login()
        {
            this.InitializeComponent();
            _apiService = ApiService.GetInstance();
        }
     private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string correo = txtUsername.Text;
            string contrasenya = txtPassword.Password;


            if (!IsValidEmail(correo))
            {
                txtMessage.Text = "Formato de correo electrónico inválido.";
                return;
            }
            try
            {
                // Enviar las credenciales al servidor
                bool loginSuccessful = await _apiService.Login(correo, contrasenya);

                if (loginSuccessful)
                {
                    // El inicio de sesión fue exitoso, puedes redirigir al usuario a la siguiente página
                    txtMessage.Text = "Inicio de sesión correcto.";
                    

                    Frame.Navigate(typeof(DashboardPage), correo);

                }
                else
                {
                    // El inicio de sesión falló, muestra un mensaje de error al usuario
                    txtMessage.Text = "Inicio de sesión fallido. Verifique sus credenciales.";
                }
            }
            catch (Exception ex)
            {
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
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }
    }
}
