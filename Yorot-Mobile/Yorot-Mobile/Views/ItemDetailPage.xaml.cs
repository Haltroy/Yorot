using System.ComponentModel;
using Xamarin.Forms;
using Yorot_Mobile.ViewModels;

namespace Yorot_Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}