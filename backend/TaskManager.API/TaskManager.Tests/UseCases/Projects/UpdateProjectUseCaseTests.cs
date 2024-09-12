using System;
using System.Threading.Tasks;
using Moq;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Domain.Entities;
using TaskManager.Tests.Repositories;
using Xunit;

namespace TaskManager.Tests.UseCases.Projects
{
    public class UpdateProjectUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldUpdateProjectNameAndInvalidateCache()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var mockCacheService = new Mock<ICacheService>();

            var useCase = new UpdateProjectUseCase(inMemoryProjectRepository, mockCacheService.Object);

            var oldName = "Old Project Name";
            var newName = "Updated Project Name";
            var project = new Project(oldName);

            // Adiciona o projeto ao repositório em memória
            await inMemoryProjectRepository.CreateAsync(project);

            // Act
            var updatedProject = await useCase.Execute(project.Id, newName);

            // Assert
            Assert.Equal(newName, updatedProject.Name);
            Assert.NotNull(updatedProject.UpdatedAt);
            mockCacheService.Verify(cache => cache.RemoveByPrefixAsync("projects_cache"), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldThrowResourceNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var mockCacheService = new Mock<ICacheService>();

            var useCase = new UpdateProjectUseCase(inMemoryProjectRepository, mockCacheService.Object);

            var nonExistentProjectId = Guid.NewGuid();
            var newName = "New Name";

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundError>(() => useCase.Execute(nonExistentProjectId, newName));
        }
    }
}
