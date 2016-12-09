using IConfessX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace IConfessX.Views
{
    public partial class LoginPage : ContentPage
    {
        UserViewModel vm;
        public LoginPage()
        {
            InitializeComponent();
            vm = UserViewModel.DefaultViewModel;

            var loginLabel_tap = new TapGestureRecognizer();
            loginLabel_tap.Tapped += LoginLabel_tap_Tapped;
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            LoginLabel.GestureRecognizers.Add(loginLabel_tap);
        }

        private async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                await DisplayAlert("Error",
                    "Check for your Internet connection", "OK");
            }
        }
        
        private async void LoginLabel_tap_Tapped(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error",
                    "Check for your Internet connection", "OK");
            }
            if (await vm.CheckForExistingUser(emailEntry.Text))
            {
                Navigation.PushAsync(new MainPage());
            }
            else
            {
                DisplayAlert("Error",
                        "This email is not registered, Please double check or register new account from below."
                        , "OK");
            }
        }


        public async void OnReg(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterationPage());


        }
    }
}
