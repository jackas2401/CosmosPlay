using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosPlay
{
    public class AuditEventFactory
    {

        public static List<AuditEvent> GeneratAuditEvents(int creates, int amendments, int deletions)
        {
            List<AuditEvent> AuditEvents = new List<AuditEvent>();

            Guid CorrelationToken = Guid.NewGuid();
            string IpAddress = "127.0.0.1";
            Random Random = new Random();
            

            for (int i = 0; i <= creates; i++)
            {
                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    Module = "BOSHost",
                    ActionType = "Create",
                    CreatedDate = DateTime.Now,
                    UserName = "Andrew",
                    IpAddress = IpAddress,
                    CorrelationToken = CorrelationToken,
                    PreviousObject = null,
                    NewObject = new User()
                    {
                        UserId = Random.Next(),
                        Username = "Andrew"
                    }

                });                
            }

            for (int i = 0; i <= amendments; i++)
            {
                int userId = Random.Next();

                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    Module = "BOSHost",
                    ActionType = "Amend",
                    CreatedDate = DateTime.Now,
                    UserName = "Andrew",
                    IpAddress = IpAddress,
                    CorrelationToken = CorrelationToken,
                    PreviousObject = new User()
                    {
                        UserId = userId,
                        Username = "Andrew"
                    },
                    NewObject = new User()
                    {
                        UserId = userId,
                        Username = "Amended Andrew"
                    }
                });          
            }

            for (int i = 0; i < deletions; i++)
            {
                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    Module = "BOSHost",
                    ActionType = "Delete",
                    CreatedDate = DateTime.Now,
                    UserName = "Andrew",
                    IpAddress = IpAddress,
                    CorrelationToken = CorrelationToken,
                    PreviousObject = new Group()
                    {
                        GroupId = Random.Next(),
                        GroupName = "Admin",
                        GroupMembers = new List<string>(){"andrew", "david", "jackman"}
                    },
                    NewObject = null
                });

                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    Module = "BOSHost",
                    ActionType = "Delete",
                    CreatedDate = DateTime.Now,
                    UserName = "Andrew",
                    IpAddress = IpAddress,
                    CorrelationToken = CorrelationToken,
                    PreviousObject = new User()
                    {
                        UserId = Random.Next(),
                        Username = "Amended Andrew"
                    },
                    NewObject = null
                });
            }

            return AuditEvents;
        }

    }
}
