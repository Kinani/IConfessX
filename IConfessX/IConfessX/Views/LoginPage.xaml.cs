using IConfessX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            LoginLabel.GestureRecognizers.Add(loginLabel_tap);
        }
        private async void LoginLabel_tap_Tapped(object sender, EventArgs e)
        {
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
