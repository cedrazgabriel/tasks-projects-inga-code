using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases
{
    public class CreateProjectUseCase(IProjectRepository projectRepository)
    {
        public async Task<Project> Execute(string name)
        {
            var project = new Project(name);

            await projectRepository.CreateAsync(project);

            return project;
        }
    }
} 
