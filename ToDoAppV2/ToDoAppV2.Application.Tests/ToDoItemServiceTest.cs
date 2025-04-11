using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ToDoAppV2.Application.Interfaces;
using ToDoAppV2.Application.Services;
using ToDoAppV2.Domain.Entities;
using Xunit;

namespace ToDoAppV2.Application.Tests
{
    public class ToDoItemServiceTest
    {
        private readonly Mock<IToDoItemRepository> _mockRepository;
        private readonly ToDoItemService _toDoItemService;

        public ToDoItemServiceTest()
        {
            _mockRepository = new Mock<IToDoItemRepository>();
            _toDoItemService = new ToDoItemService(_mockRepository.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAddOnce()
        {
            // Arrange
            var newToDoItem = new ToDoItem { Task = "New task", IsCompleted = false };
            //Act
            await _toDoItemService.AddAsync(newToDoItem);
            //Assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<ToDoItem>()), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllToDoItems()
        {
            // Arrange
            var toDoItems = new List<ToDoItem>
            {
                new ToDoItem { Id = 1, Task = "Task 1", IsCompleted = false },
                new ToDoItem { Id = 2, Task = "Task 2", IsCompleted = true },
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(toDoItems);

            // Act
            var result = await _toDoItemService.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Task 1", result.First().Task);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnToDoItem_WhenItemExists()
        {
            // Arrange
            var toDoItem = new ToDoItem { Id = 1, Task = "Task test", IsCompleted = false };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(toDoItem);

            // Act
            var result = await _toDoItemService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Task test", result.Task);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnToDoItem_WhenItemDoesNotExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(54)).ReturnsAsync((ToDoItem?)null);

            // Act
            var result = await _toDoItemService.GetByIdAsync(54);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdateOnce()
        {
            // Arrange
            var exisitingToDoItem = new ToDoItem { Id = 1, Task = "Exisiting task", IsCompleted = false };
            var updatedToDoItem = new ToDoItem { Id = 1, Task = "Updated task", IsCompleted = true };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exisitingToDoItem);

            // Act
            await _toDoItemService.UpdateAsync(updatedToDoItem);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ToDoItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotCallRepositoryUpdateOnce_WhenIdDoesNotMatch()
        {
            // Arrange
            var toDoItem = new ToDoItem { Id = 1, Task = "non-exisiting task", IsCompleted = false };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((ToDoItem?)null);
            
            // Act
            await _toDoItemService.UpdateAsync(toDoItem);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ToDoItem>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteOnce()
        {
            // Arrange
            var toDoItem = new ToDoItem { Id = 1, Task = "Task to delete", IsCompleted = true };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(toDoItem);

            // Act
            await _toDoItemService.DeleteAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallRepositoryDeleteOnce_WhenItemDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((ToDoItem?)null);

            // Act
            await _toDoItemService.DeleteAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

    }
}
