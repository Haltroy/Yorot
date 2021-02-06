using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yorot_Mobile.Services;
using Yorot_Mobile.Views;

namespace Yorot_Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
