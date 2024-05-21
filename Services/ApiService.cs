using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Windows.Foundation;
using mycooking.Models;
using System.Diagnostics;
using Windows.Storage;

namespace mycooking.Services
{
    public class ApiService
    {
        private static ApiService _instance;
        private readonly HttpClient _client = new HttpClient();
        private string _accessToken;



        public string AccessToken
        {
            get { return _accessToken; }
            private set { _accessToken = value; }
        }
        private ApiService()
        {
            // Configurar la dirección base en el constructor una sola vez
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:9098/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public static ApiService GetInstance()
        {
            // Si no existe una instancia, crear una nueva
            if (_instance == null)
            {
                _instance = new ApiService();
            }
            return _instance;
        }
        public void SetAccessToken(string token)
    {
        AccessToken = token;
    }

        //Login
        public async Task<bool> Login(string correo, string contrasenya)
        {
            try
            {
                var data = new { correo, contrasenya };
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Respuesta del servidor: " + responseData);

                    // Obtener el token de acceso desde la respuesta y guardarlo en el objeto ApiService
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseData);
                    if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        AccessToken = tokenResponse.AccessToken;

                        // Guardar el token de acceso en el almacenamiento local para persistencia
                        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                        localSettings.Values["AccessToken"] = AccessToken;

                        Debug.WriteLine("Token de acceso obtenido: " + AccessToken);
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("El inicio de sesión fue exitoso pero no se obtuvo el token.");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al iniciar sesión: " + ex.Message);
                return false;
            }
        }

        //Register
        public async Task<string> Register(string correo, string contrasenya)
        {
            var data = new { correo, contrasenya };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("register", content);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                throw new HttpRequestException($"Error al registrar. Código de estado: {response.StatusCode}");
            }
        }
        //Crear Receta
        public async Task CrearReceta(Receta receta,string rol)
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                string accessToken = localSettings.Values["AccessToken"] as string;
                // Asegurarse de que se tenga el token de autenticación
                if (string.IsNullOrEmpty(AccessToken))
                {
                    throw new InvalidOperationException("Debe iniciar sesión antes de crear una receta.");
                }


                var json = JsonConvert.SerializeObject(receta);
                Debug.WriteLine("Contenido JSON de la receta:");
                Debug.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Configurar el encabezado de autorización con el token de acceso
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AccessToken);
                Debug.WriteLine("Token de acceso enviado en el encabezado de autorización:");
                Debug.WriteLine(accessToken);

                // Realizar la solicitud con la instancia existente de HttpClient
                HttpResponseMessage response = await _client.PostAsync("recetas", content);

                Debug.WriteLine("Datos enviados a la API al crear la receta:");
                Debug.WriteLine(json);

                string responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Respuesta del servidor: " + responseContent);

                // Verificar si la respuesta indica un error de "Unauthorized"
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("No tienes permiso para realizar esta acción. Por favor, inicia sesión nuevamente.");
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Receta creada exitosamente.");
                    // Aquí puedes mostrar el mensaje de éxito o realizar otras operaciones relacionadas con el éxito
                }
                else
                {
                    throw new HttpRequestException($"Error al crear la receta. Código de estado: {response.StatusCode}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine("Error al crear la receta: " + ex.Message);

                // Manejar la excepción aquí
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al crear la receta: " + ex.Message);
                // Otras excepciones pueden ser manejadas aquí si es necesario
            }
        }


        public async Task<List<Pais>> ObtenerPaises()
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                string accessToken = localSettings.Values["AccessToken"] as string;

                // Asegurarse de que se tenga el token de autenticación
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Debe iniciar sesión antes de obtener los talleres.");
                }

                // Configurar el encabezado de autorización con el token de acceso
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);

                Debug.WriteLine("Solicitud de obtener talleres:");
                Debug.WriteLine("URL: " + _client.BaseAddress + "talleres");
                Debug.WriteLine("Método: GET");

                HttpResponseMessage response = await _client.GetAsync("paises");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("JSON recibido: " + responseData);

                    // Deserializar el JSON en PaisesResponse
                    PaisesResponse paisesResponse = JsonConvert.DeserializeObject<PaisesResponse>(responseData);
                    if (paisesResponse != null && paisesResponse.Ok && paisesResponse.Pais != null)
                    {
                        return paisesResponse.Pais;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Debug.WriteLine("Error al obtener los países. Código de estado: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al obtener los países: " + ex.Message);
                return null;
            }
        }
        //Pantalla RecetasdelMundo
        public async Task<List<Receta>> ObtenerRecetasPorPais(int paisId)
        {
            try
            {
                // Asegúrate de tener el token de acceso antes de hacer la solicitud
                string accessToken = AccessToken;
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Debe iniciar sesión antes de obtener las recetas.");
                }

                // Configura el encabezado de autorización con el token de acceso
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);

                // Realiza la solicitud GET para obtener las recetas asociadas con el país
                HttpResponseMessage response = await _client.GetAsync($"recetasporpais?paisId={paisId}");
                if (response.IsSuccessStatusCode)
                {
                    // Lee el contenido de la respuesta
                    string responseData = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("JSON recibido: " + responseData);

                    // Deserializa el JSON en un objeto RecetasResponse que contiene la lista de recetas
                    RecetasResponse recetasResponse = JsonConvert.DeserializeObject<RecetasResponse>(responseData);
                    if (recetasResponse != null && recetasResponse.Ok && recetasResponse.Recetas != null)
                    {
                        // Retorna la lista de recetas obtenida de la respuesta
                        return recetasResponse.Recetas;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Debug.WriteLine("Error al obtener las recetas. Código de estado: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al obtener las recetas: " + ex.Message);
                return null;
            }
        }




        //Crear Taller

        public async Task CrearTaller(Taller taller, string rol)
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                string accessToken = localSettings.Values["AccessToken"] as string;
                // Asegurarse de que se tenga el token de autenticación
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Debe iniciar sesión antes de crear un taller.");
                }

                var json = JsonConvert.SerializeObject(taller);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Configurar el encabezado de autorización con el token de acceso
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);
                Debug.WriteLine("Token de acceso enviado en el encabezado de autorización:");
                Debug.WriteLine(accessToken);

                // Realizar la solicitud con la instancia existente de HttpClient
                HttpResponseMessage response = await _client.PostAsync("talleres", content);

                Debug.WriteLine("Datos enviados a la API al crear el taller:");
                Debug.WriteLine(json);

                string responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Respuesta del servidor: " + responseContent);

                // Verificar si la respuesta indica un error de "Unauthorized"
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("No tienes permiso para realizar esta acción. Por favor, inicia sesión nuevamente.");
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Taller creado exitosamente.");
                    // Aquí puedes mostrar el mensaje de éxito o realizar otras operaciones relacionadas con el éxito
                }
                else
                {
                    throw new HttpRequestException($"Error al crear el taller. Código de estado: {response.StatusCode}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine("Error al crear el taller: " + ex.Message);

                // Manejar la excepción aquí
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al crear el taller: " + ex.Message);
                // Otras excepciones pueden ser manejadas aquí si es necesario
            }
        }



        public async Task<List<Taller>> ObtenerTalleresDesdeBD()
        {
            try
            {

                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                string accessToken = localSettings.Values["AccessToken"] as string;

                // Asegurarse de que se tenga el token de autenticación
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Debe iniciar sesión antes de obtener los talleres.");
                }

                // Configurar el encabezado de autorización con el token de acceso
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);

                Debug.WriteLine("Solicitud de obtener talleres:");
                Debug.WriteLine("URL: " + _client.BaseAddress + "talleres");
                Debug.WriteLine("Método: GET");

                HttpResponseMessage response = await _client.GetAsync("talleres");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Respuesta de la API al obtener talleres: " + responseData);

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);
                    if (apiResponse != null && apiResponse.Ok)
                    {
                        List<Taller> talleres = apiResponse.Talleres;
                        return talleres;
                    }
                    else
                    {
                        Debug.WriteLine("Error al obtener los talleres desde la base de datos.");
                        return null;
                    }
                }
                else
                {
                    Debug.WriteLine("Error al obtener los talleres desde la base de datos. Código de estado: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al obtener los talleres desde la base de datos: " + ex.Message);
                return null; // Maneja el error según sea necesario en tu aplicación
            }
        }



        public class TokenResponse
        {
            [JsonProperty("token")]
            public string AccessToken { get; set; }
        }

    }
}
