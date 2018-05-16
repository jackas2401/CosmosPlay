using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using Newtonsoft.Json;

namespace CosmosPlay
{

    public class AuditEvent
    {
        public enum ActionType
        {
            Default = 0,
            Create = 1,
            Update = 2,
            Delete = 3
        }

        [JsonProperty(PropertyName = "moduleId")]
        public string ModuleId { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string AuditId { get; set; }
        //[JsonProperty(PropertyName = "id", ItemConverterType = int)]
        public ActionType Action { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public Guid CorrelationToken { get; set; }
        public string PreviousObject { get; set; }
        public string NewObject { get; set; }
        public string ObjectType { get; set; }
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TimeToLive { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Text;
//using Nest;
//using Newtonsoft.Json;

//namespace CosmosPlay
//{
//    [ElasticsearchType(Name = "auditevent")]
//    public class AuditEvent
//    {
//        public string AuditId { get; set; }
//        public string Module { get; set; }
//        public string ActionType { get; set; }
//        public DateTime CreatedDate { get; set; }
//        public string UserName { get; set; }
//        [Ip]
//        public string IpAddress { get; set; }
//        public Guid CorrelationToken { get; set; }
//        [Object]
//        public Object PreviousObject { get; set; }
//        [Object]
//        public Object NewObject { get; set; }
//        public override string ToString()
//        {
//            return JsonConvert.SerializeObject(this);
//        }
//    }
//}
