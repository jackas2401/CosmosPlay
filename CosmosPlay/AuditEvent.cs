using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CosmosPlay
{
    public class AuditEvent
    {
        [JsonProperty(PropertyName = "id")]
        public string AuditId { get; set; }
        public string Module { get; set; }
        public string ActionType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public Guid CorrelationToken { get; set; }
        public Object PreviousObject { get; set; }
        public Object NewObject { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
