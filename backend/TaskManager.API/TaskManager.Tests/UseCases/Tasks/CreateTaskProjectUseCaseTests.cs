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
    public class CreateTaskUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldCreateTaskAndStoreInRepository()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var inMemoryProjectRepository = new InMemoryProjectsRepository();

            // Cria um projeto para associar à task
            var project = new Project("test project");
            await inMemoryProjectRepository.CreateAsync(project);

            var useCase = new CreateTaskUseCase(inMemoryTaskRepository, inMemoryProjectRepository);
            var taskName = "Test Task";
            var taskDescription = "Test Task Description";

            // Act
            var createdTask = await useCase.Execute(taskName, taskDescription, project.Id);

            // Assert
            var taskInRepo = await inMemoryTaskRepository.GetTaskByIdAsync(createdTask.Id);

            Assert.NotNull(taskInRepo);
            Assert.Equal(taskName, createdTask.Name);
            Assert.Equal(taskDescription, createdTask.Description);
            Assert.Equal(project.Id, createdTask.ProjectId);
            Assert.Single(inMemoryTaskRepository.Items);
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var inMemoryProjectRepository = new InMemoryProjectsRepository(); 

            var useCase = new CreateTaskUseCase(inMemoryTaskRepository, inMemoryProjectRepository);
            var taskName = "Test Task";
            var taskDescription = "Test Task Description";
            var nonExistentProjectId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() =>
                useCase.Execute(taskName, taskDescription, nonExistentProjectId));
        }
    }
}
