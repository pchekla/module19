using NUnit.Framework;
using Moq;
using SocialNetwork.BLL.Services;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.DAL.Entities;

namespace SocialNetwork.Tests;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IFriendRepository> _friendRepositoryMock;
    private Mock<MessageService> _messageServiceMock;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _friendRepositoryMock = new Mock<IFriendRepository>();
        _messageServiceMock = new Mock<MessageService>();

        _userService = new UserService();
    }

    // 🧪 Тест метода Register
    [Test]
    public void Register_WhenEmailIsInvalid_ShouldThrowArgumentNullException()
    {
        // Arrange
        var invalidUser = new UserRegistrationData
        {
            FirstName = "John",
            LastName = "Doe",
            Password = "password123",
            Email = "invalid-email"
        };

        _userRepositoryMock
            .Setup(repo => repo.FindByEmail(It.IsAny<string>()))
            .Returns((UserEntity)null);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _userService.Register(invalidUser));
    }

    [Test]
    public void Register_WhenUserAlreadyExists_ShouldThrowArgumentNullException()
    {
        // Arrange
        var existingUser = new UserRegistrationData
        {
            FirstName = "Jane",
            LastName = "Doe",
            Password = "password123",
            Email = "jane.doe@example.com"
        };

        _userRepositoryMock
            .Setup(repo => repo.FindByEmail(existingUser.Email))
            .Returns(new UserEntity { email = existingUser.Email });

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _userService.Register(existingUser));
    }

    // 🧪 Тест метода Authenticate
    [Test]
    public void Authenticate_WhenUserNotFound_ShouldThrowUserNotFoundException()
    {
        // Arrange
        var authData = new UserAuthenticationData
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(repo => repo.FindByEmail(authData.Email))
            .Returns((UserEntity)null);

        // Act & Assert
        Assert.Throws<UserNotFoundException>(() => _userService.Authenticate(authData));
    }

    [Test]
    public void Authenticate_WhenPasswordIsIncorrect_ShouldThrowWrongPasswordException()
    {
        // Arrange
        var authData = new UserAuthenticationData
        {
            Email = "user@example.com",
            Password = "wrongpassword"
        };

        var userEntity = new UserEntity
        {
            email = authData.Email,
            password = "correctpassword"
        };

        _userRepositoryMock
            .Setup(repo => repo.FindByEmail(authData.Email))
            .Returns(userEntity);

        // Act & Assert
        Assert.Throws<WrongPasswordException>(() => _userService.Authenticate(authData));
    }

    [Test]
    public void Authenticate_WhenCredentialsAreCorrect_ShouldReturnUser()
    {
        // Arrange
        var authData = new UserAuthenticationData
        {
            Email = "user@example.com",
            Password = "correctpassword"
        };

        var userEntity = new UserEntity
        {
            id = 1,
            firstname = "John",
            lastname = "Doe",
            email = authData.Email,
            password = "correctpassword"
        };

        _userRepositoryMock
            .Setup(repo => repo.FindByEmail(authData.Email))
            .Returns(userEntity);

        // Act
        var user = _userService.Authenticate(authData);

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(userEntity.id, user.Id);
        Assert.AreEqual(userEntity.firstname, user.FirstName);
    }
}
