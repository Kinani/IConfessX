using IConfessX.Models;
using IConfessX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IConfessX.Views
{
    public partial class RegisterationPage : ContentPage
    {
        UserViewModel vm;
        bool nickValid = false;
        bool emailValid = false;
        bool pswdValid = false;
        public RegisterationPage()
        {
            InitializeComponent();
            vm = UserViewModel.DefaultViewModel;
        }

        async Task AddUser(User user)
        {

            if (await vm.AddNewUserAsync(user))
            {
                await DisplayAlert("Success",
                       "You're now part of this community, Now please log in."
                       , "OK");
            }
            else
            {
                await DisplayAlert("Error",
                        "You're already registered, Please log in."
                        , "OK");
            }
        }
        public async void OnReg(object sender, EventArgs e)
        {

            if ((
                (nickValid = ValidateUsername(nicknameEntry.Text))
                && (pswdValid = ValidatePassword(passwordEntry.Text))
                && (emailValid = ValidateEmail(emailEntry.Text))
                ))
            {
                var user = new User
                {
                    Nickname = nicknameEntry.Text,
                    Password = passwordEntry.Text,
                    Email = emailEntry.Text
                };
                nickValid = false;
                emailValid = false;
                pswdValid = false;
                await AddUser(user);
            }
            else
            {
                // Show msg with the coressponding error.
                if (!nickValid)
                {
                    await DisplayAlert("Alert",
                        "You have entred invalid nickname please start with a letter and length between 1 - 24"
                        , "OK");
                    nicknameEntry.Text = string.Empty;
                    return;
                }
                if (!pswdValid)
                {
                    await DisplayAlert("Alert",
                        "You have entred invalid password please enter a password which has upper case letter and also decimal digit"
                        , "OK");
                    passwordEntry.Text = string.Empty;
                    return;
                }
                if (!emailValid)
                {
                    await DisplayAlert("Alert",
                        "You have entred invalid email"
                        , "OK");
                    emailEntry.Text = string.Empty;
                    return;
                }


            }
            nickValid = false;
            emailValid = false;
            pswdValid = false;

            Navigation.PopAsync();

        }

        #region validations

        private static bool ValidateUsername(string username)
        {
            string pattern;
            // start with a letter, allow letter or number, length between 6 to 12.
            pattern = @"^(?=[a-zA-Z])[-\w.]{0,23}([a-zA-Z\d]|(?<![-.])_)$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(username);
        }
        static bool ValidatePassword(string password)
        {
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 15;

            if (password == null) throw new ArgumentNullException();

            bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c)) hasUpperCaseLetter = true;
                    else if (char.IsLower(c)) hasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) hasDecimalDigit = true;
                }
            }

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                        ;
            return isValid;

        }
        internal static bool ValidateEmail(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }

        static Regex ValidEmailRegex = CreateValidEmailRegex();
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
