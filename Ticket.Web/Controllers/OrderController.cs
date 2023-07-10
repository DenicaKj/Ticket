using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using Ticket.Domain;
using Ticket.Service.Interface;

namespace Ticket.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            string userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._orderService.getAllOrders(userId);
            return View(result);
        }
        public IActionResult GenerateInvoice(Guid? id)
        {
            var order = _orderService.getOrderDetails(id);
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template.docx");
            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{OrderId}}", id.ToString());
            document.Content.Replace("{{OrderDate}}", order.TimeOrdered.ToString());
            StringBuilder sb = new StringBuilder();
            foreach (var item in order.TicketInOrders)
            {

                sb.AppendLine(" Number of tickets: " + item.Quantity + " Total price:" + item.Quantity * item.Ticket.TicketPrice + "\n");
            }
            document.Content.Replace("{{Tickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", order.TotalPrice.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "Invoice.pdf");
        }
        public IActionResult Details(Guid? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var result = this._orderService.getOrderDetails(id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }
    }
}
