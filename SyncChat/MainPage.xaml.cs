using SyncChat.Core;
using Xamarin.Forms;

namespace SyncChat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel();
        }
    }
}
