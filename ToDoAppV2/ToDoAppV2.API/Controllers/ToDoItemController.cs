using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoAppV2.Application.Interfaces;
using ToDoAppV2.Domain.Entities;

namespace ToDoAppV2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService _service;
        public ToDoItemController(IToDoItemService toDoItemService)
        {
            _service = toDoItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Create(ToDoItem item)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            await _service.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ToDoItem item)
        {
            if (id != item.Id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
