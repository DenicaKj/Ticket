using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.Identity;

namespace Ticket.Domain.DTO
{
    public class EditUserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string SelectedRole { get; set; }
        public List<SelectListItem> AllRoles { get; set; }
    }
}
