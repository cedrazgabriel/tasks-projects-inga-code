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
    public class UpdateTaskUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldUpdateTask_WhenTaskAndProjectExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var inMemoryProjectRepository = new InMemoryProjectsRepository();

            // Cria um projeto e uma tarefa para serem atualizados
            var project = new Project("test project");
            await inMemoryProjectRepository.CreateAsync(project);

            var task = new TaskProject("Original Task", "Original Description", project.Id);
    
            await inMemoryTaskRepository.CreateTaskAsync(task);

            var useCase = new UpdateTaskUseCase(inMemoryTaskRepository, inMemoryProjectRepository);
            var updatedName = "Updated Task";
            var updatedDescription = "Updated Description";

            // Act
            var updatedTask = await useCase.Execute(task.Id, updatedName, updatedDescription, project.Id);

            // Assert
            Assert.NotNull(updatedTask);
            Assert.Equal(updatedName, updatedTask.Name);
            Assert.Equal(updatedDescription, updatedTask.Description);
            Assert.Equal(project.Id, updatedTask.ProjectId);
            Assert.True(updatedTask.UpdatedAt > updatedTask.CreatedAt); // Verifica se o campo UpdatedAt foi atualizado
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var inMemoryProjectRepository = new InMemoryProjectsRepository();

            // Cria uma tarefa, mas nenhum projeto
            var task = new TaskProject("Original Task", "Original Description", Guid.NewGuid())
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
            await inMemoryTaskRepository.CreateTaskAsync(task);

            var useCase = new UpdateTaskUseCase(inMemoryTaskRepository, inMemoryProjectRepository);
            var updatedName = "Updated Task";
            var updatedDescription = "Updated Description";
            var nonExistentProjectId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() => useCase.Execute(task.Id, updatedName, updatedDescription, nonExistentProjectId));
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenTaskDoesNotExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var inMemoryProjectRepository = new InMemoryProjectsRepository();

            // Cria um projeto, mas nenhuma tarefa
            var project = new Project("test project");
            await inMemoryProjectRepository.CreateAsync(project);

            var useCase = new UpdateTaskUseCase(inMemoryTaskRepository, inMemoryProjectRepository);
            var updatedName = "Updated Task";
            var updatedDescription = "Updated Description";
            var nonExistentTaskId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() => useCase.Execute(nonExistentTaskId, updatedName, updatedDescription, project.Id));
        }
    }
}
