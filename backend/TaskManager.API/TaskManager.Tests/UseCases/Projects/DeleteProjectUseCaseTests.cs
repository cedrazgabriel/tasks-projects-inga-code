using System;
using System.Threading.Tasks;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Domain.Entities;
using Xunit;
using Moq;
using TaskManager.Tests.Repositories;

namespace TaskManager.Tests.UseCases.Projects
{
    public class DeleteProjectUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldMarkProjectAsDeletedAndInvalidateCache()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var mockCacheService = new Mock<ICacheService>(); 

            var useCase = new DeleteProjectUseCase(inMemoryProjectRepository, mockCacheService.Object);

            var project = new Project("Test Project");

   
            await inMemoryProjectRepository.CreateAsync(project);

            // Act
            var result = await useCase.Execute(project.Id);

            // Assert
            var deletedProject = await inMemoryProjectRepository.GetProjectByIdAsync(project.Id);

          
            Assert.NotNull(deletedProject.DeletedAt);
            Assert.True(result);

            // Verifica se o cache foi invalidado
            mockCacheService.Verify(cache => cache.RemoveByPrefixAsync("projects_cache"), Times.Once);
        }
    }
}
