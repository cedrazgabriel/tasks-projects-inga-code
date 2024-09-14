using Moq;
using System;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Tests.Repositories;
using Xunit;

namespace TaskManager.Tests.UseCases.Tasks
{
    public class GetTaskByIdUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();

            // Cria uma tarefa para ser buscada
            var task = new TaskProject("Test Task", "Test Description", Guid.NewGuid());
           
            await inMemoryTaskRepository.CreateTaskAsync(task);

            var useCase = new GetTaskByIdUseCase(inMemoryTaskRepository);

            // Act
            var result = await useCase.Execute(task.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Id, result.Id);
            Assert.Equal(task.Name, result.Name);
            Assert.Equal(task.Description, result.Description);
            Assert.Equal(task.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenTaskDoesNotExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var useCase = new GetTaskByIdUseCase(inMemoryTaskRepository);
            var nonExistentTaskId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() => useCase.Execute(nonExistentTaskId));
        }
    }
}
