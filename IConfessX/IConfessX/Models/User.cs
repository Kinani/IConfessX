using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IConfessX.Models
{
    public class User
    {
        string id;
        string nickname;
        string email;
        string pswd;
        DateTime createdAt;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        [JsonProperty(PropertyName = "pswd")]
        public string Password
        {
            get { return pswd; }
            set { pswd = value; }
        }

        [JsonProperty(PropertyName = "createdAt")]
        [CreatedAt]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }
    }
}
