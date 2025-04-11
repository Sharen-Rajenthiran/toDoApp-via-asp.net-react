using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoAppV2.Domain.Entities;

namespace ToDoAppV2.Application.Interfaces
{
    public interface IToDoItemRepository
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync();
        Task<ToDoItem?> GetByIdAsync(int id);
        Task AddAsync(ToDoItem item);
        Task UpdateAsync(ToDoItem item);
        Task DeleteAsync(int toDoListId);

    }
}
