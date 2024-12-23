using System.ComponentModel.DataAnnotations;
using SocialNetwork.BLL.Models;
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
}