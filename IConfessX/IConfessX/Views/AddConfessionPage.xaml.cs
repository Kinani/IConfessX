using IConfessX.Models;
using IConfessX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IConfessX.Views
{
    public partial class AddConfessionPage : ContentPage
    {
        ThreadViewModel vm;
        public AddConfessionPage()
        {
            InitializeComponent();
            newThreadContent.SetBinding(Editor.TextProperty, "TContent");
            vm = ThreadViewModel.DefaultViewModel;
            newThreadContent.TextChanged += (s, e) =>
            {
                var newText = e.NewTextValue;
                if (newText != string.Empty)
                {
                    ConfessBtn.IsEnabled = true;
                }
                else
                {
                    ConfessBtn.IsEnabled = false;
                }
            };
        }
        async Task AddThread(Thread item)
        {
            item.UserID = App.USERID;
            await vm.SaveThreadAsync(item);
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(newThreadContent.Text))
            {
                var thread = new Thread
                {
                    TContent = newThreadContent.Text,
                    TRate = 0,
                    UserID = App.USERID
                };
                await AddThread(thread);
            }

            newThreadContent.Text = string.Empty;
            newThreadContent.Unfocus();
            Navigation.PopAsync();
        }
    }
}
