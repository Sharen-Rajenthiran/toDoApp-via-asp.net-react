using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoAppV2.Application.Interfaces;
using ToDoAppV2.Domain.Entities;
using ToDoAppV2.Infrastructure.Data;

namespace ToDoAppV2.Infrastructure.Repositories
{
    public class ToDoItemRepository : IToDoItemRepository
    {   
        private readonly AppDbContext _appDbContext;

        public ToDoItemRepository(AppDbContext appDbContext) 
        { 
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(ToDoItem item)
        {
            _appDbContext.ToDoItems.Add(item);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int toDoListId)
        {
            var item = await _appDbContext.ToDoItems.FindAsync(toDoListId);
            if (item != null)
            {
                _appDbContext.ToDoItems.Remove(item);
                await _appDbContext.SaveChangesAsync();
            }
            
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync()
        {
            return await _appDbContext.ToDoItems.ToListAsync();
        }


        public async Task<ToDoItem?> GetByIdAsync(int id)
        {
            return await _appDbContext.ToDoItems.FindAsync(id);
            
        }

        public async Task UpdateAsync(ToDoItem item)
        {
            _appDbContext.ToDoItems.Update(item);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
