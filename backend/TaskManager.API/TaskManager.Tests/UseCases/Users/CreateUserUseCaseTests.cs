using System;
using System.Threading.Tasks;
using Moq;
using TaskManager.Application.Cryptograph.Contracts;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Application.UseCases.Users;
using TaskManager.Domain.Entities;
using Xunit;

namespace TaskManager.Tests.UseCases.Users
{
    public class CreateUserUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldCreateUserAndCollaborator_WhenUserDoesNotExist()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockCollaboratorRepository = new Mock<ICollaboratorRepository>();
            var mockHashGenerator = new Mock<IHashGenerator>();
            var mockAuthService = new Mock<IAuthService>();

            var useCase = new CreateUserUseCase(
                mockUserRepository.Object,
                mockCollaboratorRepository.Object,
                mockHashGenerator.Object,
                mockAuthService.Object);

            var username = "testuser";
            var password = "password123";
            var hashedPassword = "hashedPassword";
            var jwtToken = "jwtToken123";

            mockUserRepository.Setup(repo => repo.FindByUserNameAsync(username))
                .ReturnsAsync((User)null); 

            mockHashGenerator.Setup(hash => hash.HashAsync(password))
                .ReturnsAsync(hashedPassword); 

            mockAuthService.Setup(auth => auth.GenerateJwtToken(username, It.IsAny<string>()))
                .ReturnsAsync(jwtToken);

            // Act
            var result = await useCase.Execute(username, password);

            // Assert
            
            mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
            mockCollaboratorRepository.Verify(repo => repo.CreateAsync(It.IsAny<Collaborator>()), Times.Once);
           
            Assert.Equal(jwtToken, result);
        }

        [Fact]
        public async Task Execute_ShouldThrowUserAlreadyExistsError_WhenUserAlreadyExists()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockCollaboratorRepository = new Mock<ICollaboratorRepository>();
            var mockHashGenerator = new Mock<IHashGenerator>();
            var mockAuthService = new Mock<IAuthService>();

            var useCase = new CreateUserUseCase(
                mockUserRepository.Object,
                mockCollaboratorRepository.Object,
                mockHashGenerator.Object,
                mockAuthService.Object);

            var username = "existinguser";
            var password = "password123";

            var existingUser = new User(username, "existingHashedPassword");

            mockUserRepository.Setup(repo => repo.FindByUserNameAsync(username))
                .ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistsError>(() => useCase.Execute(username, password));

            // Verifica que os repositórios de criação de usuário e colaborador não foram chamados
            mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
            mockCollaboratorRepository.Verify(repo => repo.CreateAsync(It.IsAny<Collaborator>()), Times.Never);
        }
    }
}
