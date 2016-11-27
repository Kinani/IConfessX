using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IConfessX.Models
{
    public class Thread
    {
        string id;
        string userID;
        string tContent;
        int tRate;
        DateTime createdAt;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "userID")]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [JsonProperty(PropertyName = "tContent")]
        public string TContent
        {
            get { return tContent; }
            set { tContent = value; }
        }

        [JsonProperty(PropertyName = "tRate")]
        public int TRate
        {
            get { return tRate; }
            set { tRate = value; }
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
