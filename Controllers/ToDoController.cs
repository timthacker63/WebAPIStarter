using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebAPIStarter.Models;

namespace WebAPIStarter.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase {
        private readonly ToDoContext _context;

        public TodoController (ToDoContext context) {
            _context = context;
            if (_context.ToDoItems.Count () == 0) {
                _context.ToDoItems.Add (new ToDoItem { Name = "Items1" });
                _context.SaveChanges ();
            }
        }

        [HttpGet]
        public ActionResult<List<ToDoItem>> GetAll () {
            return _context.ToDoItems.ToList ();
        }

        [HttpGet ("{id}", Name = "GetToDo")]
        public ActionResult<ToDoItem> GetByID (long id) {
            var item = _context.ToDoItems.Find (id);
            if (item == null) {
                return NotFound ();
            }
            return item;
        }

        [HttpPut ("{id}")]
        public IActionResult Update (long id, ToDoItem item) {
            var todo = _context.ToDoItems.Find (id);
            if (todo == null) {
                return NotFound ();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;
            _context.ToDoItems.Update (todo);
            _context.SaveChanges ();
            return NoContent ();

        }

        [HttpPost]
        public IActionResult Create (ToDoItem item) {
            _context.ToDoItems.Add (item);
            _context.SaveChanges ();

            return CreatedAtRoute ("GetTodo", new { id = item.Id }, item);
        }

    }
}