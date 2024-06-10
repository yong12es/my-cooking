using mycooking.ViewModels;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class FiltrarIngredientesPage : Page
    {
        public FiltrarIngredientesViewModel ViewModel { get; set; }

        public FiltrarIngredientesPage()
        {
            this.InitializeComponent();
            ViewModel = new FiltrarIngredientesViewModel();
            this.DataContext = ViewModel;
        }



        private async void FiltrarButtom_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string ingredientes = IngredientesTextBox.Text;
            await ViewModel.ObtenerRecetasFiltradas(ingredientes);
        }
    }
}
