using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ToDoAppV2.Application.Interfaces;
using ToDoAppV2.Domain.Entities;
using ToDoAppV2.Infrastructure.Data;
using ToDoAppV2.Infrastructure.Repositories;
using Xunit;

namespace ToDoAppV2.Infrastructure.Tests
{
    public class ToDoItemRepositoryTests
    {
        private readonly IServiceProvider _serviceProvider;
        public ToDoItemRepositoryTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            serviceCollection.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        private async Task<ToDoItemRepository> GetRepositoryAsync()
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
            return new ToDoItemRepository(context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddItem()
        {
            // Arrange
            var repository = await GetRepositoryAsync();
            var newToDoItem = new ToDoItem { Id = 1, Task = "Test", IsCompleted = false };

            // Act
            await repository.AddAsync(newToDoItem);
            var items = await repository.GetByIdAsync(1);

            // Assert
            Assert.Equal("Test", items?.Task);

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllItems()
        {
            // Arrange
            var repository = await GetRepositoryAsync();
            var newToDoItem1 = new ToDoItem { Id = 1, Task = "Test 1", IsCompleted = false };
            var newToDoItem2 = new ToDoItem { Id = 2, Task = "Test 2", IsCompleted = false };
            await repository.AddAsync(newToDoItem1);
            await repository.AddAsync(newToDoItem2);

            // Act
            var items = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem_WhenExist()
        {
            // Arrange
            var repository = await GetRepositoryAsync();
            var newToDoItem = new ToDoItem { Id = 1, Task = "Test to get", IsCompleted = false };
            await repository.AddAsync(newToDoItem);

            // Act
            var item = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(newToDoItem.Task, item.Task);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExist()
        {
            // Arrange
            var repository = await GetRepositoryAsync();

            // Act
            var item = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(item);

        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateItem()
        {
            // Arrange
            var repository = await GetRepositoryAsync();
            var newToDoItem = new ToDoItem { Id = 1, Task = "Task to update", IsCompleted = false };
            await repository.AddAsync(newToDoItem);
            newToDoItem.Task = "Updated";

            // Act
            await repository.UpdateAsync(newToDoItem);
            var updatedItem = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(updatedItem);
            Assert.Equal("Updated", updatedItem?.Task);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteItem()
        {
            // Arrange
            var repository = await GetRepositoryAsync();
            var newToDoItem = new ToDoItem { Id = 1, Task = "Task to delete", IsCompleted = false };
            await repository.AddAsync(newToDoItem);

            // Act
            await repository.DeleteAsync(1);
            
            // ASsert
            var item = await repository.GetByIdAsync(1);
            Assert.Null(item);

        }
    }
}