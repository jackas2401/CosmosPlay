using System;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CosmosTableAPI
{
    public class AuditEvent : TableEntity
    {
        private string _module;
        public string Module
        {
            get => _module;
            set
            {
                _module = value;
                this.PartitionKey = _module;
            }
        }

        private string _auditId;
        public string AuditId
        {
            get => _auditId;
            set
            {
                _auditId = value;
                this.RowKey = _auditId;
            }
        }
        public string ActionType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public Guid CorrelationToken { get; set; }
        public string PreviousObject { get; set; }
        public string NewObject { get; set; }
        //public override string ToString()
        //{
        //    return JsonConvert.SerializeObject(this);
        //}
    }
}
