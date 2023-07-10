using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DomainModels;

namespace Ticket.Service.Interface
{
    public interface IEmailService
    {
        Task SendEmails(List<EmailMessage> allMails);
    }
}
