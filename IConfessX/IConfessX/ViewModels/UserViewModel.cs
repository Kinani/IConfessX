using IConfessX.Common;
using IConfessX.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IConfessX.ViewModels
{
    public class UserViewModel
    {
        static UserViewModel defaultInstance = new UserViewModel();
        MobileServiceClient client;

        IMobileServiceTable<User> userTable;

        private UserViewModel()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

            this.userTable = client.GetTable<User>();
        }

        public static UserViewModel DefaultViewModel
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
                return userTable
                    is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<User>;
            }
        }

        public async Task<bool> AddNewUserAsync(User nUser)
        {
            try
            {
                bool userexists = await CheckForExistingUser(nUser.Email);
                if (!userexists)
                {
                    await userTable.InsertAsync(nUser);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                // TODO Handle ya ostaaaaaz!!!
                return false;
            }
        }

        // Can be used for log in :))
        public async Task<bool> CheckForExistingUser(string newMail)
        {
            bool result = false;
            try
            {

                IEnumerable<User> PotentialIdiotUser = await userTable.
                    Where(user => user.Email == newMail).ToCollectionAsync();

                if (PotentialIdiotUser.Count() > 0)
                {
                    result = true;
                    App.USERID = PotentialIdiotUser.FirstOrDefault().Id;
                }

                else
                    result = false;

                return result;
            }
            catch
            {
                return result;
            }

        }

    }
}
