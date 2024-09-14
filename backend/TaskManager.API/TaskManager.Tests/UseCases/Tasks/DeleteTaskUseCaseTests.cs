using Moq;
using System;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Tests.Repositories;
using Xunit;

namespace TaskManager.Tests.UseCases.Tasks
{
    public class DeleteTaskUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldMarkTaskAsDeleted()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();

            // Cria uma tarefa para ser deletada
            var task = new TaskProject("Test Task", "Test Description", Guid.NewGuid())
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
            await inMemoryTaskRepository.CreateTaskAsync(task);

            var useCase = new DeleteTaskUseCase(inMemoryTaskRepository);

            // Act
            await useCase.Execute(task.Id);

            // Assert
            var deletedTask = await inMemoryTaskRepository.GetTaskByIdAsync(task.Id);
            Assert.NotNull(deletedTask);
            Assert.NotNull(deletedTask.DeletedAt);
            Assert.True(deletedTask.DeletedAt <= DateTime.UtcNow);
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenTaskDoesNotExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var useCase = new DeleteTaskUseCase(inMemoryTaskRepository);
            var nonExistentTaskId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() => useCase.Execute(nonExistentTaskId));
        }
    }
}
