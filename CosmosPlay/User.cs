﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CosmosPlay
{
    public class User
    {
        public long UserId { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
