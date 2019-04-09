using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorialCreateAWebApiWithWebApiAspNetCoreMvc.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TutorialCreateAWebApiWithWebApiAspNetCoreMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext  _context;


        

        public TodoController(TodoContext context)
        {
            _context = context;

            CreateItemIfContextIsEmpty();
            

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem; 
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id , TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var toDoItemForDelete = await _context.TodoItems.FindAsync(id);

            if (toDoItemForDelete == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(toDoItemForDelete);
            await _context.SaveChangesAsync();

            return NoContent();

        }



        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);

        }







        private void CreateItemIfContextIsEmpty()
        {

            if (_context.TodoItems.Count() ==0)
            { 
                
            _context.TodoItems.Add(new TodoItem { Name = "Item1" });
            _context.SaveChanges();

            }
        }

        


    }
}
