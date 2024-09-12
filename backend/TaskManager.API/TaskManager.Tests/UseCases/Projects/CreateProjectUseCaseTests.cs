using Moq;
using System;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Domain.Entities;
using TaskManager.Tests.Repositories;
using Xunit;

namespace TaskManager.Tests.UseCases.Projects
{
    public class CreateProjectUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldCreateProjectAndStoreInRepositoryAndClearCache()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var mockCacheService = new Mock<ICacheService>();

            var useCase = new CreateProjectUseCase(inMemoryProjectRepository, mockCacheService.Object);
            var projectName = "Test Project";

            // Act
            var createdProject = await useCase.Execute(projectName);

            // Assert
           
            Assert.Equal(inMemoryProjectRepository.projects?.Count() , 1); 
            Assert.Equal(projectName, createdProject.Name); 

            // Verifica se o cache foi invalidado
            mockCacheService.Verify(cache => cache.RemoveByPrefixAsync("projects_cache"), Times.Once);
        }
    }
}
