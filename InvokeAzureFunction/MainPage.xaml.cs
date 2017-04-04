using InvokeAzureFunction.ViewModels;
using Windows.UI.Xaml.Controls;

namespace InvokeAzureFunction
{
    sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = this.Resources["viewmodel"] as MainViewModel;
        }
    }
}