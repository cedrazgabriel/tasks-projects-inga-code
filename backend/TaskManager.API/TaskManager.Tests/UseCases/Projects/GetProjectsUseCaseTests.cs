using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Domain.Entities;
using TaskManager.API.DTO.Response;
using Xunit;
using TaskManager.Tests.Repositories;

namespace TaskManager.Tests.UseCases.Projects
{
    public class GetProjectsUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldReturnPaginatedProjects_FromRepository_WhenCacheIsEmpty()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var mockCacheService = new Mock<ICacheService>();

            var useCase = new GetProjectsUseCase(inMemoryProjectRepository, mockCacheService.Object);

            var project1 = new Project("Project 1");
            var project2 = new Project("Project 2");
            var project3 = new Project("Project 3");

            // Adiciona projetos ao repositório
            await inMemoryProjectRepository.CreateAsync(project1);
            await inMemoryProjectRepository.CreateAsync(project2);
            await inMemoryProjectRepository.CreateAsync(project3);

            // Configura o mock para retornar null do cache
            mockCacheService.Setup(cache => cache.GetAsync<PaginatedResult<Project?>>(It.IsAny<string>()))
                .ReturnsAsync((PaginatedResult<Project?>)null);

            int page = 1;
            int pageSize = 2;

            // Act
            var result = await useCase.Execute(page, pageSize);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(3, result.TotalRecords);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(1, result.Page); 

            // Verifica se o cache foi chamado para setar os dados
            mockCacheService.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<PaginatedResult<Project>>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnCachedProjects_WhenCacheIsNotEmpty()
        {
            // Arrange
            var inMemoryProjectRepository = new InMemoryProjectsRepository();
            var mockCacheService = new Mock<ICacheService>();

            var useCase = new GetProjectsUseCase(inMemoryProjectRepository, mockCacheService.Object);

            var cachedProjects = new PaginatedResult<Project>
            {
                Items = new List<Project> { new Project("Cached Project") },
                Page = 1,
                PageSize = 1,
                TotalRecords = 1
            };

            // Configura o mock para retornar os projetos em cache
            mockCacheService.Setup(cache => cache.GetAsync<PaginatedResult<Project>>(It.IsAny<string>()))
                .ReturnsAsync(cachedProjects);

            int page = 1;
            int pageSize = 1;

            // Act
            var result = await useCase.Execute(page, pageSize);

            // Assert
            Assert.Single(result.Items); 
            Assert.Equal("Cached Project", result.Items[0].Name); 

            // Verifica se o cache não foi atualizado
            mockCacheService.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<PaginatedResult<Project>>(), It.IsAny<TimeSpan>()), Times.Never);
        }
    }
}
