using mycooking.Services;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            string correo = txtEmail.Text;
            string contrasenya = txtPassword.Password;
            string confirmarContrasenya = txtConfirmPassword.Password;
            string rol = "";

            if (!IsValidEmail(correo))
            {
                txtMessage.Text = "Formato de correo electrónico inválido.";
                return;
            }

            if (contrasenya != confirmarContrasenya)
            {
                txtMessage.Text = "Las contraseñas no coinciden.";
                return;
            }

            try
            {
                var response = await _apiService.Register(correo, contrasenya, rol);

                txtMessage.Text = "Registro exitoso. Ahora puedes iniciar sesión.";

                Frame.Navigate(typeof(Login));
            }
            catch (HttpRequestException ex)
            {
                txtMessage.Text = "Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Error: " + ex.Message;
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, pattern);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }
    }
}
