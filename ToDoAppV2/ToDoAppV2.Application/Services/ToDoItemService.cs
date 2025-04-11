using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoAppV2.Application.Interfaces;
using ToDoAppV2.Domain.Entities;

namespace ToDoAppV2.Application.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _repository;

        public ToDoItemService(IToDoItemRepository repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(ToDoItem item)
        {
           await _repository.AddAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return;
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ToDoItem?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ToDoItem item)
        {
            var existingItem = await _repository.GetByIdAsync(item.Id);
            if (existingItem == null) return;

            existingItem.Task = item.Task;
            existingItem.IsCompleted = item.IsCompleted;
            
            await _repository.UpdateAsync(existingItem);

        }
    }
}
