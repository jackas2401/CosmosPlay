using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CosmosTableAPI
{
    public class Group
    {
        public long GroupId { get; set; }
        public string GroupName { get; set; }
        public List<string> GroupMembers { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
