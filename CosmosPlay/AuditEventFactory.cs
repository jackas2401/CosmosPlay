using System;
using System.Collections.Generic;
using System.Text;
using static CosmosPlay.AuditEvent;

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
            int ttl = 60 * 60;
            string ModuleId = "Profiles";

            for (int i = 0; i <= creates; i++)
            {
                User newObject = new User
                {
                    UserId = Random.Next(),
                    Username = "Jerry"
                };

                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    ModuleId = ModuleId,
                    Action = ActionType.Create,
                    ActionReason = "New starter has joined as per request 1243",
                    CreatedDate = DateTime.Now,
                    UserName = "Tom",
                    IpAddress = IpAddress,
                    CorrelationToken = Guid.NewGuid(),
                    PreviousObject = null,
                    NewObject = newObject.ToString(),
                    ObjectType = newObject.GetType().AssemblyQualifiedName,
                    TimeToLive = ttl
                });                
            }

            for (int i = 0; i <= amendments; i++)
            {
                int userId = Random.Next();

                User previousObject = new User()
                {
                    UserId = userId,
                    Username = "Andrew"
                };

                User newObject = new User()
                {
                    UserId = userId,
                    Username = "Amended Andrew"
                };

                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    ModuleId = ModuleId,
                    Action = ActionType.Update,
                    ActionReason = "Amended username as per request 2234",
                    CreatedDate = DateTime.Now,
                    UserName = "Andrew",
                    IpAddress = IpAddress,
                    CorrelationToken = CorrelationToken,
                    PreviousObject = previousObject.ToString(),
                    NewObject = newObject.ToString(),
                    ObjectType = newObject.GetType().AssemblyQualifiedName,
                    TimeToLive = ttl
                });          
            }

            for (int i = 0; i < deletions; i++)
            {
                Group previousObject = new Group()
                {
                    GroupId = Random.Next(),
                    GroupName = "Admin",
                    GroupMembers = new List<string>() {"andrew", "david", "jackman"}
                };

                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    ModuleId = ModuleId,
                    Action = ActionType.Delete,
                    ActionReason = "Tidy up groups as no current members",
                    CreatedDate = DateTime.Now,
                    UserName = "Andrew",
                    IpAddress = IpAddress,
                    CorrelationToken = CorrelationToken,
                    PreviousObject = previousObject.ToString(),
                    NewObject = null,
                    ObjectType = previousObject.GetType().AssemblyQualifiedName,
                    TimeToLive = ttl
                });

                User previousUserObject = new User()
                {
                    UserId = Random.Next(),
                    Username = "Amended Andrew"
                };

                AuditEvents.Add(new AuditEvent()
                {
                    AuditId = Guid.NewGuid().ToString(),
                    ModuleId = ModuleId,
                    Action = ActionType.Delete,
                    ActionReason = "Leave request 23432423",
                    CreatedDate = DateTime.Now,
                    UserName = "Andrew",
                    IpAddress = IpAddress,
                    CorrelationToken = CorrelationToken,
                    PreviousObject = previousUserObject.ToString(),
                    NewObject = null,
                    ObjectType = previousUserObject.GetType().AssemblyQualifiedName,
                    TimeToLive = ttl
                });
            }

            return AuditEvents;
        }

    }
}
