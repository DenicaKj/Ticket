using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Domain.Identity;
using Ticket.Domain.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using ExcelDataReader;
using Ticket.Domain.DomainModels;

namespace Ticket.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly UserManager<TicketApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<TicketApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

            var model = new EditUserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                SelectedRole = userRole.FirstOrDefault(),
                AllRoles = allRoles
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ImportUsers(List<UserRegistrationDto> model)
        {
            var file = Request.Form.Files[0];
            if (file == null || file.Length == 0)
            {
                return RedirectToAction("Index");
            }
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString() == "FirstName") continue;
                        var dto = new UserRegistrationDto
                        {
                            Name = reader.GetValue(0).ToString(),
                            LastName = reader.GetValue(1).ToString(),
                            Email = reader.GetValue(2).ToString(),
                            Password = reader.GetValue(3).ToString(),
                            PhoneNumber = reader.GetValue(4).ToString(),

                        };
                        var Role = reader.GetValue(5).ToString();
                        var userCheck = _userManager.FindByEmailAsync(dto.Email).Result;
                        if (userCheck == null)
                        {
                            var user = new TicketApplicationUser
                            {
                                FirstName = dto.Name,
                                LastName = dto.LastName,
                                UserName = dto.Email.Split('@')[0],
                                NormalizedUserName = dto.Email,
                                Email = dto.Email,
                                EmailConfirmed = true,
                                PhoneNumberConfirmed = true,
                                PhoneNumber = dto.PhoneNumber,
                                UserCart = new ShoppingCart()
                            };
                            var result = await _userManager.CreateAsync(user, dto.Password);

                            if (result.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(user, Role);
                            }
                            else
                            {
                                if (result.Errors.Count() > 0)
                                {
                                    foreach (var error in result.Errors)
                                    {
                                        ModelState.AddModelError("message", error.Description);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("message", "Email already exists.");
                        }
                    }
                }
            }
            return RedirectToAction("Index");

        }

        // POST: Role/EditRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = await _userManager.GetRolesAsync(user);
            var selectedRole = model.SelectedRole;

            await _userManager.RemoveFromRolesAsync(user, userRole);
            await _userManager.AddToRoleAsync(user, selectedRole);

            return RedirectToAction("Index");
        }
    }
}