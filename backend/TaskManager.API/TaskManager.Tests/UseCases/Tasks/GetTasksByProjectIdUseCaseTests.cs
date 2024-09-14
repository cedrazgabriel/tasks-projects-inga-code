using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Tests.Repositories;
using Xunit;

namespace TaskManager.Tests.UseCases.Tasks
{
    public class GetTasksByProjectIdUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldReturnPaginatedTasks_WhenProjectExists()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var inMemoryProjectRepository = new InMemoryProjectsRepository();

            // Cria um projeto para associar as tasks
            var project = new Project("test project");
            await inMemoryProjectRepository.CreateAsync(project);

            // Adiciona algumas tasks ao projeto
            for (int i = 1; i <= 3; i++)
            {
                var task = new TaskProject($"Task {i}", $"Description {i}", project.Id);
                await inMemoryTaskRepository.CreateTaskAsync(task);
            }

            var useCase = new GetTasksByProjectIdUseCase(inMemoryTaskRepository, inMemoryProjectRepository);
            var page = 1;
            var pageSize = 2;

            // Act
            var paginatedTasks = await useCase.Execute(project.Id, page, pageSize);

            // Assert
            Assert.NotNull(paginatedTasks);
            Assert.Equal(2, paginatedTasks.Items.Count); 
            Assert.True(paginatedTasks.TotalRecords > 2); 
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var inMemoryProjectRepository = new InMemoryProjectsRepository(); // Sem adicionar um projeto

            var useCase = new GetTasksByProjectIdUseCase(inMemoryTaskRepository, inMemoryProjectRepository);
            var nonExistentProjectId = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() => useCase.Execute(nonExistentProjectId, page, pageSize));
        }
    }
}
