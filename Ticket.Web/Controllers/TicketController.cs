using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticket.Domain.DomainModels;
using Ticket.Domain.DTO;
using Ticket.Domain.Relations;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;
using Ticket.Web.Data;

namespace Ticket.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IMovieService _moviesService;
        private readonly IRepository<TicketInMovie> _repository;
        public TicketController(ITicketService ticketService, IMovieService moviesService, IRepository<TicketInMovie> repository)
        {
            _ticketService = ticketService;
            _moviesService = moviesService;
            _repository = repository;
        }

        // GET: Ticket
        public IActionResult Index(DateTime? selectedDate)
        {
            var tickets = selectedDate.HasValue
            ? _ticketService.GetByDateValid(selectedDate.Value)
            : _ticketService.GetAllNotSoldProducts();
            return View(tickets);
        }

        // GET: Ticket/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketProd = this._ticketService.GetDetailsForProduct(id);
            if (ticketProd == null)
            {
                return NotFound();
            }
            return View(ticketProd);
        }

        // GET: Ticket/Create
        public IActionResult Create()
        {
            var movies = new SelectList(_moviesService.GetAllMovies(), "Id", "Title");
            ViewBag.Movies = movies;
            return View();
        }

        // POST: Ticket/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TicketNumber,TicketProjectionRoom,TicketPrice,TicketRating,Id,dateValid")] TicketProd ticketProd, Guid movieId)
        {
            if (ModelState.IsValid)
            {
                ticketProd.Id = Guid.NewGuid();
                this._ticketService.CreateNewProduct(ticketProd,movieId);
                return RedirectToAction(nameof(Index));
            }
            return View(ticketProd);
        }

        // GET: Ticket/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketProd = this._ticketService.GetDetailsForProduct(id);
            if (ticketProd == null)
            {
                return NotFound();
            }
            return View(ticketProd);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("TicketNumber,TicketProjectionRoom,TicketPrice,TicketRating,MovieId,dateValid,Id")] TicketProd ticketProd)
        {
            if (id != ticketProd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._ticketService.UpdeteExistingProduct(ticketProd);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticketProd.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticketProd);
        }

        // GET: Ticket/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketProd = this._ticketService.GetDetailsForProduct(id);
            if (ticketProd == null)
            {
                return NotFound();
            }

            return View(ticketProd);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._ticketService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddTicketToCart(Guid id)
        {
            var result = this._ticketService.GetShoppingCartInfo(id);

            return View(result);
        }

        public IActionResult exportToExcel(string Genre)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Tickets.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                        workbook.Worksheets.Add("Tickets");
                    worksheet.Cell(1, 1).Value = "Movie";
                    worksheet.Cell(1, 2).Value = "TicketNumber";
                    worksheet.Cell(1, 3).Value = "Date Valid";
                    worksheet.Cell(1, 4).Value = "Price";
          
                    var i = 2;
                    foreach (var item in _ticketService.GetAllProducts())
                    {
                        if (item.TicketInMovie.First().Movie.Genre == Genre) { 
                        worksheet.Cell(i, 1).Value = item.TicketInMovie.First().Movie.Title;
                        worksheet.Cell(i, 2).Value = item.TicketNumber;
                        worksheet.Cell(i, 3).Value = item.dateValid;
                        worksheet.Cell(i, 4).Value = item.TicketPrice;
                        i++;}
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }

                }
            }
            catch (Exception ex) { }
            return View("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCart(AddToShoppingCardDto model)
        {


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._ticketService.AddToShoppingCart(model, userId);

            if (result)
            {
                return RedirectToAction("Index", "Ticket");
            }
            return View(model);
        }
        private bool TicketExists(Guid id)
        {
            return this._ticketService.GetDetailsForProduct(id) != null;
        }
    }
}
