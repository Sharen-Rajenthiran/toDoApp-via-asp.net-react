using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoAppV2.API.Controllers;
using ToDoAppV2.Application.Interfaces;
using ToDoAppV2.Domain.Entities;
using Xunit;

namespace ToDoAppV2.API.Tests
{
    public class ToDoItemControllerTests
    {
        private readonly Mock<IToDoItemService> _mockService;
        private readonly ToDoItemController _controller;

        public ToDoItemControllerTests()
        {
            _mockService = new Mock<IToDoItemService>();
            _controller = new ToDoItemController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WhenItemExists()
        {
            // Arrange
            var mockService = new Mock<IToDoItemService>();
            var toDoItems = new List<ToDoItem>
            {
                new ToDoItem { Id = 1, Task = "Task 1", IsCompleted = false },
                new ToDoItem { Id = 2, Task = "Task 2", IsCompleted = true }
            };
            mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(toDoItems);

            var controller = new ToDoItemController(mockService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItems = Assert.IsType<List<ToDoItem>>(okResult.Value);
            Assert.Equal(2, returnedItems.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenItemExists()
        {
            // Arrange
            var mockService = new Mock<IToDoItemService>();
            var toDoItem = new ToDoItem { Id = 1, Task = "Task 1", IsCompleted = false };
            mockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(toDoItem);

            var controller = new ToDoItemController(mockService.Object);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItem = Assert.IsType<ToDoItem>(okResult.Value);
            Assert.Equal(1, returnedItem.Id);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenItemIsCreated()
        {
            // Arrange
            var mockService = new Mock<IToDoItemService>();
            var toDoItem = new ToDoItem { Id = 1, Task = "New Task", IsCompleted = false };
            mockService.Setup(service => service.AddAsync(toDoItem)).Returns(Task.CompletedTask);

            var controller = new ToDoItemController(mockService.Object);

            // Act
            var result = await controller.Create(toDoItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedItem = Assert.IsType<ToDoItem>(createdAtActionResult.Value);
            Assert.Equal(1, returnedItem.Id);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mockService = new Mock<IToDoItemService>();
            var toDoItem = new ToDoItem { Id = 1, Task = "Updated Task", IsCompleted = false };
            mockService.Setup(service => service.UpdateAsync(toDoItem)).Returns(Task.CompletedTask);

            var controller = new ToDoItemController(mockService.Object);

            // Act
            var result = await controller.Update(1, toDoItem);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }
        

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenItemIsDeleted()
        {
            // Arrange
            var toDoItem = new ToDoItem { Id = 1, Task = "Task to delete", IsCompleted = true };
            var mockService = new Mock<IToDoItemService>();
            mockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(toDoItem);
            mockService.Setup(service => service.DeleteAsync(1)).Returns(Task.CompletedTask);
            var controller = new ToDoItemController(mockService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            mockService.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IToDoItemService>();
            mockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((ToDoItem?)null);  
            mockService.Setup(service => service.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);  

            var controller = new ToDoItemController(mockService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);  
            mockService.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never); 
        }





    }
}
