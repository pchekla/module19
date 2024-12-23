using System.ComponentModel.DataAnnotations;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;

namespace SocialNetwork.BLL.Services;

public class UserService
{
    IUserRepository userRepository;

    public UserService()
    {
        userRepository = new UserRepository();
    }

    public void Register(UserRegistrationData userRegistrationData)
    {
        if (String.IsNullOrEmpty(userRegistrationData.FirstName))
        {
            throw new ArgumentNullException("First name is required");
        }

        if (String.IsNullOrEmpty(userRegistrationData.LastName))
        {
            throw new ArgumentNullException("Last name is required");
        }

        if (String.IsNullOrEmpty(userRegistrationData.Password))
        {
            throw new ArgumentNullException("Password is required");
        }

        if (String.IsNullOrEmpty(userRegistrationData.Email))
        {
            throw new ArgumentNullException("Email is required");
        }

        if (userRegistrationData.Password.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long");
        }

        if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
        {
            throw new ArgumentException("Email is not valid");
        }

        if (userRepository.FindByEmail(userRegistrationData.Email) != null)
        {
            throw new ArgumentException("User with this email already exists");
        }

        var userEntity = new UserEntity()
        {
            firstname = userRegistrationData.FirstName,
            lastname = userRegistrationData.LastName,
            password = userRegistrationData.Password,
            email = userRegistrationData.Email
        };

        if (this.userRepository.Create(userEntity) == 0)
        {
            throw new Exception("User was not created");
        }
    }

    public User Authenticate(UserAuthenticationData userAuthenticationData)
    {
        var findUserEntity = userRepository.FindByEmail(userAuthenticationData.Email);
        if (findUserEntity is null) throw new UserNotFoundException();

        if (findUserEntity.password != userAuthenticationData.Password)
            throw new WrongPasswordException();

        return ConstructUserModel(findUserEntity);
    }

    public User FindByEmail(string email)
    {
        var findUserEntity = userRepository.FindByEmail(email);
        if (findUserEntity is null) throw new UserNotFoundException();

        return ConstructUserModel(findUserEntity);
    }

    public void Update(User user)
    {
        var updatableUserEntity = new UserEntity()
        {
            id = user.Id,
            firstname = user.FirstName,
            lastname = user.LastName,
            password = user.Password,
            email = user.Email,
            photo = user.Photo,
            favorite_movie = user.FavoriteMovie,
            favorite_book = user.FavoriteBook
        };

        if (this.userRepository.Update(updatableUserEntity) == 0)
            throw new Exception();
    }

    private User ConstructUserModel(UserEntity userEntity)
    {
        return new User(userEntity.id,
                        userEntity.firstname,
                        userEntity.lastname,
                        userEntity.password,
                        userEntity.email,
                        userEntity.photo,
                        userEntity.favorite_movie,
                        userEntity.favorite_book);
    }
}