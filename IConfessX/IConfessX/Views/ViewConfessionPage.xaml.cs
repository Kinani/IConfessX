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
    public partial class ViewConfessionPage : ContentPage
    {
        ThreadViewModel vm;
        public ViewConfessionPage()
        {
            InitializeComponent();
            newThreadContent.SetBinding(Editor.TextProperty, "TContent");
            vm = ThreadViewModel.DefaultViewModel;
            newThreadContent.TextChanged += (s, e) =>
            {
                var newText = e.NewTextValue;
                if (newText != string.Empty)
                {
                    UpdateBtn.IsEnabled = true;
                }
                else
                {
                    UpdateBtn.IsEnabled = false;
                }
            };
        }
        async Task AddThread(Thread item)
        {
            await vm.SaveThreadAsync(item);
        }

        public async void OnUpdate(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(newThreadContent.Text))
            {
                Thread thread = BindingContext as Thread;
                thread.TContent = newThreadContent.Text;

                await AddThread(thread);
            }

            newThreadContent.Text = string.Empty;
            newThreadContent.Unfocus();
            Navigation.PopAsync();
            // Navigate Back and refresh
        }
    }
}
