using IConfessX.Models;
using IConfessX.ViewModels;
using IConfessX.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IConfessX
{
    public partial class MainPage : ContentPage
    {
        ThreadViewModel vm;
        public MainPage()
        {
            InitializeComponent();
            vm = ThreadViewModel.DefaultViewModel;
            this.ToolbarItems.Add(new ToolbarItem()
            {
                Icon = "addNew.png",
                Command = new Command(() => Navigation.PushAsync(new AddConfessionPage()))
            });

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await RefreshItems(true, syncItems: true);
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            await RefreshItems(false, true); // Careful !!!
        }
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(true, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }
        private async void OnTapped(object sender, ItemTappedEventArgs e)
        {
            var thread = e.Item as Thread;
            var viewConfessionsPage = new ViewConfessionPage();
            viewConfessionsPage.BindingContext = thread;
            await Navigation.PushAsync(viewConfessionsPage);
        }

        public async void OnVoteUp(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var thread = mi.CommandParameter as Thread;
            thread.TRate++;
            await vm.SaveThreadAsync(thread);
            await RefreshItems(false, true);
        }
        public async void OnVoteDown(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var thread = mi.CommandParameter as Thread;
            thread.TRate--;
            await vm.SaveThreadAsync(thread);
            await RefreshItems(false, true);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                threadList.ItemsSource = await vm.GetThreadItemsAsync(syncItems);
            }
        }
        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}
