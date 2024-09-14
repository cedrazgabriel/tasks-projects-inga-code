using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Tests.Repositories;
using Xunit;

namespace TaskManager.Tests.UseCases.Tasks
{
    public class GetTasksUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldReturnPaginatedTasks()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();

            // Adiciona algumas tasks ao repositório
            for (int i = 1; i <= 5; i++)
            {
                var task = new TaskProject($"Task {i}", $"Description {i}", Guid.NewGuid())
                {
                    CreatedAt = DateTime.UtcNow
                };
                await inMemoryTaskRepository.CreateTaskAsync(task);
            }

            var useCase = new GetTasksUseCase(inMemoryTaskRepository);
            var page = 1;
            var pageSize = 3;

            // Act
            var paginatedTasks = await useCase.Execute(page, pageSize);

            // Assert
            Assert.NotNull(paginatedTasks);
            Assert.Equal(pageSize, paginatedTasks.Items.Count); 
            Assert.True(paginatedTasks.TotalRecords >= 5); 
        }

        [Fact]
        public async Task Execute_ShouldReturnEmptyPaginatedResult_WhenNoTasksExist()
        {
            // Arrange
            var inMemoryTaskRepository = new InMemoryTaskRepository();
            var useCase = new GetTasksUseCase(inMemoryTaskRepository);
            var page = 1;
            var pageSize = 3;

            // Act
            var paginatedTasks = await useCase.Execute(page, pageSize);

            // Assert
            Assert.NotNull(paginatedTasks);
            Assert.Empty(paginatedTasks.Items); 
            Assert.Equal(0, paginatedTasks.TotalRecords); 
        }
    }
}
