using IConfessX.Common;
using IConfessX.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IConfessX.ViewModels
{
    public class ThreadViewModel
    {
        static ThreadViewModel defaultInstance = new ThreadViewModel();
        MobileServiceClient client;

        IMobileServiceSyncTable<Thread> threadTable;

        private ThreadViewModel()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            var store = new MobileServiceSQLiteStore(Constants.offlineDbPath);

            store.DefineTable<Thread>();
            this.client.SyncContext.InitializeAsync(store);

            this.threadTable = client.GetSyncTable<Thread>();
        }

        public static ThreadViewModel DefaultViewModel
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get
            {
                return threadTable
                    is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Thread>;
            }
        }

        public async Task<ObservableCollection<Thread>> GetThreadItemsAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await this.SyncAsync();
                }

                IEnumerable<Thread> threads = await threadTable
                    .Where(threadItem => threadItem.TContent != string.Empty)
                    .OrderByDescending(threadItem => threadItem.CreatedAt) // Test
                    .ToEnumerableAsync();
                return new ObservableCollection<Thread>(threads);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                //TODO Handle!!
            }
            catch (Exception e)
            {

                //TODO Handle!!
            }
            return null;
        }

        public async Task<ObservableCollection<Thread>> GetThreadItemsAsync(string userId, bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await this.SyncAsync();
                }

                IEnumerable<Thread> threads = await threadTable
                    .Where(threadItem => threadItem.UserID == userId)
                    .OrderByDescending(threadItem => threadItem.CreatedAt) // Test
                    .ToEnumerableAsync();
                return new ObservableCollection<Thread>(threads);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                //TODO Handle!!
            }
            catch (Exception e)
            {

                //TODO Handle!!
            }
            return null;
        }
        public async Task SaveThreadAsync(Thread thread)
        {
            if (thread.Id == null)
            {
                await threadTable.InsertAsync(thread);
            }
            else
            {
                await threadTable.UpdateAsync(thread);
            }
        }
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.threadTable.PullAsync(
                    "allThreadItems",
                    this.threadTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    // TODO Handle Error !!!

                    //Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
