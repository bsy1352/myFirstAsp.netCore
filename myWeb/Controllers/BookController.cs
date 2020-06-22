using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWeb.Model;

namespace myWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Book")]
    [ApiController]
    public class BookController : Controller
    {

        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { Book = await _db.Book.ToListAsync() });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> OnGet(int id)
        {
            return Json(new { Book = await _db.Book.FindAsync(id) });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Book.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFromDb == null)
            {
                return NotFound();


            }

            _db.Book.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var bookFromDb = await _db.Book.ToListAsync();
            if (bookFromDb == null)
            {
                return NotFound();

            }

            _db.Book.RemoveRange(bookFromDb);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book Book)
        {
            if (ModelState.IsValid)
            {
                await _db.Book.AddAsync(Book);
                await _db.SaveChangesAsync();

                return Created("api/Book", Book);
            }
            else
            {
                return NoContent();
            }

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] Book Book)
        {
            if (ModelState.IsValid)
            {
                var BookFromDb = await _db.Book.FindAsync(id);
                BookFromDb.Name = Book.Name;
                BookFromDb.Author = Book.Author;
                BookFromDb.ISBN = Book.ISBN;

                await _db.SaveChangesAsync();

                
                return Accepted("api/Book/" + id, Book);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
