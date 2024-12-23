using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;

namespace SocialNetwork;

class Program
{
    public static UserService userService = new UserService();
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the social network!");

        while (true)
        {
            Console.WriteLine("Please enter your first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Please enter your last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();

            Console.WriteLine("Please enter your email:");
            string email = Console.ReadLine();

            UserRegistrationData userRegistrationData = new UserRegistrationData()
            {
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Email = email
            };

            try
            {
                userService.Register(userRegistrationData);
                Console.WriteLine("User registered successfully!");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}