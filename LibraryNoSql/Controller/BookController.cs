using LibraryNoSql.ApiModel;
using LibraryNoSql.Model;
using LibraryNoSql.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;

namespace LibraryNoSql.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookRepository bookRepository;
        public BookController(BookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var books = bookRepository.GetAll();
            return Ok(books);
        }

        [HttpPost]
        [Route("insert")]
        public IActionResult Insert(BookApiModel model)
        {
            var dbBook = bookRepository.Insert(
                new Book()
                {
                    Title = model.Title,
                    Pages = model.Pages,
                    Author = model.Author
                });
            return Ok(dbBook);
        }

        [HttpGet]
        [Route("getByUser")]
        public IActionResult GetByUser(Guid userId)
        {
            var dbBooks = bookRepository.GetByUser(userId);
            return Ok(dbBooks);
        }

        [HttpGet]
        [Route("getById")]
        public IActionResult GetById(ObjectId bookId)
        {
            var dbBook = bookRepository.GetById(bookId);
            return Ok(dbBook);
        }

        [HttpDelete]
        [Route("deleteById")]
        public IActionResult DeleteById(ObjectId bookId)
        {
            bookRepository.Delete(bookId);
            return Ok();
        }

        [HttpPut]
        [Route("give")]
        public IActionResult GiveBookToUser(GiveBookToUserApiModel giveBookToUserApiModel)
        {
            try
            {
                bookRepository.GiveBookToUser(giveBookToUserApiModel.bookId, giveBookToUserApiModel.userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok("Book given successfully");
        }

        [HttpPut]
        [Route("retrieve")]
        public IActionResult RetrieveBookFromUser(ObjectId bookId)
        {
            try
            {
                bookRepository.RetrieveBookFromUser(bookId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Book returned to library successfully");
        }
    }
}
