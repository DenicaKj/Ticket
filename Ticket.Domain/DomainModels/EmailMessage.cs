using System;
using System.Collections.Generic;
using System.Text;

namespace Ticket.Domain.DomainModels
{
    public class EmailMessage : BaseEntity
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailTo { get; set; }
        public Boolean Status { get; set; }
    }
}
