using System;
using System.Threading.Tasks;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Domain.Entities;
using Xunit;
using TaskManager.Tests.Repositories;

namespace TaskManager.Tests.UseCases.Projects
{
    public class GetProjectByIdUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldReturnProject_WhenProjectExists()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var useCase = new GetProjectByIdUseCase(inMemoryProjectRepository);


            var project = new Project("Test Project");

            // Adiciona o projeto ao repositório
            await inMemoryProjectRepository.CreateAsync(project);

            // Act
            var result = await useCase.Execute(project.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.Id, result.Id);
            Assert.Equal("Test Project", result.Name);
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var useCase = new GetProjectByIdUseCase(inMemoryProjectRepository);

            var nonExistentProjectId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() => useCase.Execute(nonExistentProjectId));
        }
    }
}
