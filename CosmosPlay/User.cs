using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CosmosPlay
{
    public class User
    {
        [JsonProperty(PropertyName = "userId")]
        public long UserId { get; set; }
        public string Username { get; set; }        
        
    }
}
