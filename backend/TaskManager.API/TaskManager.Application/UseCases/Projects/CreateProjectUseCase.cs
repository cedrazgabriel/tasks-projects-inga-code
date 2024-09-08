using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Projects
{
    public class CreateProjectUseCase(IProjectRepository projectRepository)
    {
        public async Task<Project> Execute(string name)
        {
            var project = new Project(name);
            project.CreatedAt = DateTime.UtcNow;

            await projectRepository.CreateAsync(project);

            return project;
        }
    }
}
